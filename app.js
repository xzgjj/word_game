const maxStack = 9999;

const itemCatalog = [
    ["Wood", "木材"],
    ["Stone", "石子"],
    ["FlowerSeed", "花种"],
    ["RiverShell", "河贝"],
    ["Fish", "鱼"],
    ["EmotionShard", "表情碎片"],
    ["OldCartridge", "旧卡带"],
    ["StarCore", "星屑灯芯"],
    ["Sticker", "贴纸"],
];

const emptySlotCount = 3;

const blueprintNames = {
    Bridge: "木桥",
    FlowerBed: "花圃",
    ForestSign: "林间路牌",
    WoodFence: "木栅栏",
    RiverLamp: "河岸灯",
    StickerWall: "贴纸墙",
    ArcadeBase: "游戏机底座",
    CraftBench: "工作台",
    CustomBuilding: "自建建筑",
};

const exchanges = [
    { id: "exchange-emotion-shard", label: "换表情碎片", output: "EmotionShard", count: 1, cost: { Wood: 1, FlowerSeed: 1 } },
    { id: "exchange-old-cartridge", label: "换旧卡带", output: "OldCartridge", count: 1, cost: { RiverShell: 1, Fish: 1 } },
    { id: "exchange-star-core", label: "换星屑灯芯", output: "StarCore", count: 1, cost: { Stone: 2, RiverShell: 1 } },
    { id: "exchange-sticker", label: "换贴纸", output: "Sticker", count: 1, cost: { Fish: 2, EmotionShard: 1 } },
];

const buildRecipes = {
    Bridge: { Wood: 1 },
    FlowerBed: { FlowerSeed: 2, Stone: 1 },
    ForestSign: { Wood: 1, EmotionShard: 1 },
    WoodFence: { Wood: 2 },
    RiverLamp: { Wood: 1, StarCore: 1 },
    ArcadeBase: { Stone: 2, StarCore: 1 },
    CraftBench: { Wood: 2, Stone: 1 },
};

const buildSlots = {
    Bridge: { x: 2, y: 0 },
    FlowerBed: { x: 0, y: 0 },
    ForestSign: { x: 1, y: 0 },
    RiverLamp: { x: 2, y: 1 },
    ArcadeBase: { x: 3, y: 0 },
    WoodFence: { x: 0, y: 1 },
    CraftBench: { x: 1, y: 1 },
};

const state = {
    location: "木屋前",
    hint: "先看看木屋前木牌，图纸会被记录下来。",
    inventory: Object.fromEntries(itemCatalog.map(([id]) => [id, 0])),
    unlockedBlueprints: new Set(),
    knownSystems: new Set(),
    placedBuildings: [],
    builtCount: 0,
    customBuildUnlocked: false,
    miniGameUnlocked: false,
    activeMiniGame: false,
    miniGameStickers: 0,
    stickerWallCount: 0,
    timeOfDay: "morning",
};

const $ = (selector) => document.querySelector(selector);
const $$ = (selector) => Array.from(document.querySelectorAll(selector));

const locationLabel = $("#locationLabel");
const hintLabel = $("#hintLabel");
const recordLabel = $("#recordLabel");
const inventoryGrid = $("#inventoryGrid");
const recipeList = $("#recipeList");
const blueprintRow = $("#blueprintRow");
const buildActions = $("#buildActions");
const signboardState = $("#signboardState");
const customState = $("#customState");
const customBuildButton = $("#customBuildButton");
const bridgeState = $("#bridgeState");
const promptText = $("#promptText");
const avatarToken = $("#avatarToken");
const stickerWall = $("#stickerWall");
const arcadeState = $("#arcadeState");
const pixelConsole = $("#pixelConsole");
const pixelStage = $("#pixelStage");
const startMiniGame = $("#startMiniGame");
const exitDoor = $("#exitDoor");
const timeNote = $("#timeNote");

function clampAdd(itemId, count) {
    state.inventory[itemId] = Math.min(maxStack, state.inventory[itemId] + count);
}

function canAfford(cost) {
    return Object.entries(cost).every(([itemId, count]) => state.inventory[itemId] >= count);
}

function spend(cost) {
    if (!canAfford(cost)) {
        return false;
    }

    Object.entries(cost).forEach(([itemId, count]) => {
        state.inventory[itemId] -= count;
    });
    return true;
}

function formatCost(cost) {
    return Object.entries(cost)
        .map(([itemId, count]) => `${itemName(itemId)} ${count}`)
        .join(" / ");
}

function itemName(itemId) {
    return itemCatalog.find(([id]) => id === itemId)?.[1] || itemId;
}

function openSignboard() {
    state.knownSystems.add("signboard");
    ["Bridge", "FlowerBed", "ForestSign"].forEach((blueprint) => state.unlockedBlueprints.add(blueprint));
    state.location = "木屋前木牌";
    state.hint = "木牌记录了可换物资、初始图纸和建设数量。";
    prompt("木牌不是任务列表；它像小镇工具，告诉你材料能换什么、图纸解锁到哪里。");
    render();
}

function discoverArcade() {
    state.knownSystems.add("arcade");
    state.miniGameUnlocked = state.inventory.OldCartridge > 0;
    state.location = "空地游戏机";
    state.hint = state.miniGameUnlocked ? "旧卡带已在包里，游戏机菜单可进入。" : "发现了游戏机菜单，但还缺旧卡带。";
    prompt(state.hint);
    render();
}

function gatherForest() {
    clampAdd("Wood", 2);
    clampAdd("FlowerSeed", 1);
    state.location = "旁友森林";
    state.hint = "森林树枝和草丛花种进入物品栏。";
    moveAvatar("24%", "38%");
    prompt("木材可修桥，花种可做花圃。材料来源和建设需求保持一一对应。");
    render();
}

function gatherRiverbank() {
    clampAdd("Stone", 2);
    clampAdd("RiverShell", 1);
    state.location = "河岸";
    state.hint = "河岸石子和浅水河贝进入物品栏。";
    moveAvatar("47%", "64%");
    prompt("石子用于花圃、游戏机底座和星屑灯芯兑换；河贝可换旧卡带或星屑灯芯。");
    render();
}

function fishRiver() {
    clampAdd("Fish", 2);
    state.location = "河水";
    state.hint = "鱼进入物品栏，可兑换旧卡带或贴纸。";
    moveAvatar("58%", "56%");
    prompt("河水只提供鱼，不改变主世界地图数量。");
    render();
}

function exchange(recipeId) {
    openSignboard();
    const recipe = exchanges.find((entry) => entry.id === recipeId);
    if (!recipe || !spend(recipe.cost)) {
        prompt("材料不足，先去森林、河岸或河水补齐基础材料。");
        render();
        return;
    }

    clampAdd(recipe.output, recipe.count);
    if (recipe.output === "OldCartridge" && state.knownSystems.has("arcade")) {
        state.miniGameUnlocked = true;
    }
    state.hint = `${recipe.label}完成：${itemName(recipe.output)} +${recipe.count}`;
    prompt(`${recipe.label}完成。成长物资来自木牌兑换，基础材料仍来自主世界。`);
    render();
}

function placeBuild(blueprint) {
    if (!state.unlockedBlueprints.has(blueprint)) {
        prompt("图纸未解锁。先看木牌，或通过修桥和小游戏获得后续图纸。");
        return;
    }

    const slot = buildSlots[blueprint];
    if (state.placedBuildings.some((placed) => placed.x === slot.x && placed.y === slot.y)) {
        prompt("这个位置已经放过建设物。");
        return;
    }

    const cost = buildRecipes[blueprint];
    if (!spend(cost)) {
        prompt(`材料不足：${blueprintNames[blueprint]}需要 ${formatCost(cost)}。`);
        render();
        return;
    }

    state.placedBuildings.push({ blueprint, ...slot });
    state.builtCount += 1;
    if (blueprint === "Bridge") {
        state.unlockedBlueprints.add("WoodFence");
        state.unlockedBlueprints.add("RiverLamp");
        bridgeState.textContent = "木桥已修好";
    }
    unlockCustomIfReady();
    state.hint = `${blueprintNames[blueprint]}已放置。`;
    prompt(`${blueprintNames[blueprint]}写入 placedBuildings。建设反馈来自材料消耗和地图节点变化。`);
    render();
}

function unlockCustomIfReady() {
    if (state.builtCount >= 3) {
        state.customBuildUnlocked = true;
        state.unlockedBlueprints.add("CustomBuilding");
    }
}

function placeCustomBuild() {
    if (!state.customBuildUnlocked) {
        prompt("自建建筑还没开启。先放置 3 个图纸物品。");
        return;
    }

    const cost = { RiverShell: 1 };
    if (!spend(cost)) {
        prompt("受控自建需要河贝 1。");
        render();
        return;
    }

    state.placedBuildings.push({ blueprint: "CustomBuilding", x: 4, y: 1, material: "RiverShell", size: "1x1" });
    state.builtCount += 1;
    state.hint = "1x1 河贝小屋已试建。";
    prompt("自建仍是受控组合，不进入完整体素沙盒。");
    render();
}

function startMiniGameFlow() {
    discoverArcade();
    if (!state.miniGameUnlocked || state.inventory.OldCartridge < 1) {
        prompt("游戏机菜单已发现，但需要旧卡带。去木牌用河贝和鱼兑换。");
        return;
    }

    state.activeMiniGame = true;
    state.miniGameStickers = 0;
    state.location = "像素小游戏";
    state.hint = "收集 3 个贴纸后出口点亮。";
    $$(".pixel-sticker").forEach((button) => button.classList.remove("collected"));
    document.body.dataset.mode = "pixel";
    pixelStage.classList.add("active");
    prompt("进入像素模式后规则变了：目标是贴纸和出口，不再采集主世界材料。");
    render();
}

function collectMiniGameSticker(button) {
    if (!state.activeMiniGame || button.classList.contains("collected")) {
        prompt("先从游戏机菜单进入像素小游戏。");
        return;
    }

    button.classList.add("collected");
    state.miniGameStickers = Math.min(3, state.miniGameStickers + 1);
    state.hint = state.miniGameStickers >= 3 ? "出口已点亮，可以回到主世界。" : `贴纸 ${state.miniGameStickers}/3`;
    prompt(state.hint);
    render();
}

function finishMiniGame() {
    if (!state.activeMiniGame || state.miniGameStickers < 3) {
        prompt("出口还没亮。收集 3 个贴纸后再离开。");
        return;
    }

    state.activeMiniGame = false;
    state.miniGameStickers = 0;
    state.stickerWallCount += 1;
    clampAdd("Sticker", 1);
    state.unlockedBlueprints.add("StickerWall");
    state.unlockedBlueprints.add("ArcadeBase");
    state.location = "空地游戏机";
    state.hint = "贴纸墙更新，后续图纸已记录到木牌。";
    document.body.dataset.mode = "world";
    pixelStage.classList.remove("active");
    prompt("小游戏完成后回到主世界，木屋贴纸墙 +1，木牌新增贴纸墙和游戏机底座图纸。");
    render();
}

function setTimeOfDay(timeKey) {
    state.timeOfDay = timeKey;
    document.body.dataset.time = timeKey;
    const notes = {
        morning: "清晨提示更清楚",
        noon: "午后适合看建设空位",
        night: "夜晚能看见电子入口和河岸灯",
    };
    timeNote.textContent = notes[timeKey];
    prompt(notes[timeKey]);
    render();
}

function moveAvatar(left, top) {
    avatarToken.style.left = left;
    avatarToken.style.top = top;
}

function prompt(text) {
    promptText.textContent = text;
}

function renderInventory() {
    inventoryGrid.innerHTML = "";
    itemCatalog.forEach(([itemId, label]) => {
        const slot = document.createElement("div");
        slot.className = "inventory-slot";
        slot.innerHTML = `<span>${label}</span><strong>${state.inventory[itemId]}</strong>`;
        inventoryGrid.appendChild(slot);
    });

    for (let index = 0; index < emptySlotCount; index += 1) {
        const slot = document.createElement("div");
        slot.className = "inventory-slot empty";
        slot.innerHTML = `<span>预留</span><strong>空</strong>`;
        inventoryGrid.appendChild(slot);
    }
}

function renderRecipes() {
    recipeList.innerHTML = "";
    exchanges.forEach((recipe) => {
        const button = document.createElement("button");
        button.type = "button";
        button.className = "recipe-button";
        button.disabled = !canAfford(recipe.cost);
        button.dataset.recipe = recipe.id;
        button.innerHTML = `<span>${recipe.label}</span><small>${formatCost(recipe.cost)}</small>`;
        button.addEventListener("click", () => exchange(recipe.id));
        recipeList.appendChild(button);
    });
}

function renderBlueprints() {
    blueprintRow.innerHTML = "";
    if (state.unlockedBlueprints.size === 0) {
        blueprintRow.innerHTML = "<span>打开木牌后记录初始图纸</span>";
        return;
    }

    Array.from(state.unlockedBlueprints).forEach((blueprint) => {
        const chip = document.createElement("span");
        chip.textContent = blueprintNames[blueprint];
        blueprintRow.appendChild(chip);
    });
}

function renderBuildActions() {
    buildActions.innerHTML = "";
    Object.keys(buildRecipes).forEach((blueprint) => {
        const button = document.createElement("button");
        button.type = "button";
        button.disabled = !state.unlockedBlueprints.has(blueprint);
        button.textContent = blueprintNames[blueprint];
        button.addEventListener("click", () => placeBuild(blueprint));
        buildActions.appendChild(button);
    });
}

function renderWorld() {
    $$(".build-slot, .bridge-slot").forEach((button) => {
        const blueprint = button.dataset.build;
        button.classList.toggle("placed", state.placedBuildings.some((placed) => placed.blueprint === blueprint));
    });

    stickerWall.textContent = `贴纸墙 ${state.stickerWallCount}`;
    signboardState.textContent = state.knownSystems.has("signboard") ? "已打开" : "未查看";
    customState.textContent = state.customBuildUnlocked ? "自建已开启" : `建设 ${state.builtCount}/3 后开启`;
    customBuildButton.disabled = !state.customBuildUnlocked;
    arcadeState.textContent = state.knownSystems.has("arcade")
        ? (state.miniGameUnlocked ? "旧卡带已就绪，可自主进入。" : "已发现菜单，缺旧卡带。")
        : "还没查看空地游戏机。";
    startMiniGame.disabled = !state.miniGameUnlocked && state.inventory.OldCartridge < 1;
    exitDoor.classList.toggle("ready", state.activeMiniGame && state.miniGameStickers >= 3);
}

function render() {
    unlockCustomIfReady();
    locationLabel.textContent = state.location;
    hintLabel.textContent = state.hint;
    recordLabel.textContent = `建设 ${state.builtCount}/3 · ${state.customBuildUnlocked ? "自建已开启" : "自建未开启"}`;
    renderInventory();
    renderRecipes();
    renderBlueprints();
    renderBuildActions();
    renderWorld();
    $$("#timeButtons button").forEach((button) => button.classList.toggle("active", button.dataset.time === state.timeOfDay));
}

$$("[data-action]").forEach((button) => {
    button.addEventListener("click", () => {
        const action = button.dataset.action;
        if (action === "open-signboard") openSignboard();
        if (action === "gather-forest") gatherForest();
        if (action === "gather-riverbank") gatherRiverbank();
        if (action === "fish-river") fishRiver();
        if (action === "open-arcade") discoverArcade();
    });
});

$$("[data-build]").forEach((button) => {
    button.addEventListener("click", () => placeBuild(button.dataset.build));
});

$$(".pixel-sticker").forEach((button) => button.addEventListener("click", () => collectMiniGameSticker(button)));
$$("[data-time]").forEach((button) => button.addEventListener("click", () => setTimeOfDay(button.dataset.time)));
customBuildButton.addEventListener("click", placeCustomBuild);
startMiniGame.addEventListener("click", startMiniGameFlow);
exitDoor.addEventListener("click", finishMiniGame);

render();
