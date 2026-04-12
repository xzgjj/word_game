const state = {
    location: "木屋前",
    objective: "目标：去空地看看发光的游戏机",
    wood: 0,
    stickers: 0,
    shards: 0,
    bridgeRepaired: false,
    inPixelMode: false,
    completed: false,
    emotion: "joy",
};

const emotionMeta = {
    joy: {
        emoji: "😄",
        desc: "欢笑模式：短冲刺，适合越过一格缺口和快速拾取。",
        gradient: "linear-gradient(180deg, #f4d35e, #ff6f61)",
    },
    calm: {
        emoji: "🙂",
        desc: "安静模式：短暂漂浮，适合通过移动平台和观察隐藏提示。",
        gradient: "linear-gradient(180deg, #bdf7f0, #49b7c7)",
    },
    hype: {
        emoji: "🤩",
        desc: "激动模式：高跳触发按钮，打开像素小游戏的出口门。",
        gradient: "linear-gradient(180deg, #ff6f61, #f4d35e)",
    },
};

const locationLabel = document.getElementById("locationLabel");
const objectiveLabel = document.getElementById("objectiveLabel");
const inventoryLabel = document.getElementById("inventoryLabel");
const avatarToken = document.getElementById("avatarToken");
const promptBubble = document.getElementById("promptBubble");
const modeDesc = document.getElementById("modeDesc");
const bridgeNode = document.getElementById("bridgeNode");
const pixelHero = document.getElementById("pixelHero");
const stickerCounter = document.getElementById("stickerCounter");
const exitDoor = document.getElementById("exitDoor");
const stepList = document.getElementById("stepList");

function updateHud() {
    locationLabel.textContent = state.location;
    objectiveLabel.textContent = state.objective;
    inventoryLabel.textContent = `木材 ${state.wood} / 贴纸 ${state.stickers} / 碎片 ${state.shards}`;
    stickerCounter.textContent = `贴纸 ${state.stickers}/3`;
    bridgeNode.classList.toggle("repaired", state.bridgeRepaired);
    exitDoor.classList.toggle("ready", state.stickers >= 3);

    const steps = stepList.querySelectorAll("span");
    steps[1].classList.toggle("done", state.wood > 0 || state.shards > 0);
    steps[2].classList.toggle("done", state.bridgeRepaired);
    steps[3].classList.toggle("done", state.inPixelMode || state.completed);
    steps[4].classList.toggle("done", state.completed);
}

function moveAvatar(left, top) {
    avatarToken.style.left = left;
    avatarToken.style.top = top;
}

function setEmotion(mode) {
    const meta = emotionMeta[mode];
    state.emotion = mode;
    avatarToken.textContent = meta.emoji;
    avatarToken.style.background = meta.gradient;
    pixelHero.textContent = meta.emoji;
    modeDesc.textContent = meta.desc;

    document.querySelectorAll(".mode-button").forEach((button) => {
        button.classList.toggle("active", button.dataset.mode === mode);
    });
}

function handleWorldAction(action) {
    if (action === "home") {
        state.location = "木屋前";
        state.objective = state.completed ? "目标：查看贴纸墙的新贴纸" : "目标：去空地看看发光的游戏机";
        promptBubble.textContent = state.completed
            ? "贴纸墙已经更新，第一轮闭环完成。"
            : "木屋是安全起点。先去森林拾取木材，再修桥过河。";
        avatarToken.classList.remove("pixel");
        moveAvatar("236px", "250px");
    }

    if (action === "forage") {
        state.location = "旁友森林";
        state.wood = Math.max(state.wood, 1);
        state.shards = Math.max(state.shards, 1);
        state.objective = "目标：带着木材去河边修桥";
        promptBubble.textContent = "拾取到木材和表情碎片。桥只需要 1 个木材，避免早期系统变复杂。";
        moveAvatar("58%", "310px");
    }

    if (action === "arcade") {
        if (!state.bridgeRepaired) {
            if (state.wood < 1) {
                state.objective = "目标：先去森林找木材";
                promptBubble.textContent = "河流挡住了去空地的路。先去旁友森林拾取木材。";
                updateHud();
                return;
            }

            state.bridgeRepaired = true;
            state.location = "河边木桥";
            state.objective = "目标：木桥修好了，前往空地游戏机";
            promptBubble.textContent = "木桥补齐。下一次点击空地游戏机，会确认进入像素模式。";
            moveAvatar("50%", "355px");
            updateHud();
            return;
        }

        state.location = "空地游戏机";
        state.inPixelMode = true;
        state.objective = "目标：像素模式中收集 3 个贴纸";
        promptBubble.textContent = "拾取游戏卡带，主角像素化。点击小游戏里的贴纸，收齐后点出口返回主世界。";
        avatarToken.classList.add("pixel");
        moveAvatar("74%", "456px");
    }

    updateHud();
}

function collectSticker(button) {
    if (!state.inPixelMode || button.classList.contains("collected")) {
        promptBubble.textContent = "先从空地游戏机进入像素模式，再收集贴纸。";
        return;
    }

    button.classList.add("collected");
    state.stickers += 1;
    state.objective = state.stickers >= 3 ? "目标：贴纸已集齐，打开出口" : "目标：继续收集贴纸";
    promptBubble.textContent = state.stickers >= 3
        ? "3 个贴纸已收齐。现在可以点击出口回到主世界。"
        : "贴纸飞入 HUD。小游戏保持轻松，不设置惩罚。";
    updateHud();
}

function exitMiniGame() {
    if (state.stickers < 3) {
        promptBubble.textContent = "出口还没亮。先收集 3 个贴纸。";
        return;
    }

    state.inPixelMode = false;
    state.completed = true;
    state.location = "空地游戏机";
    state.objective = "目标：回木屋查看贴纸墙";
    promptBubble.textContent = "小游戏完成，回到主世界。游戏机点亮，木屋贴纸墙会新增贴纸。";
    avatarToken.classList.remove("pixel");
    moveAvatar("74%", "456px");
    updateHud();
}

document.querySelectorAll(".mode-button").forEach((button) => {
    button.addEventListener("click", () => setEmotion(button.dataset.mode));
});

document.querySelectorAll(".map-node").forEach((node) => {
    node.addEventListener("click", () => handleWorldAction(node.dataset.action));
});

document.querySelectorAll(".sticker").forEach((button) => {
    button.addEventListener("click", () => collectSticker(button));
});

exitDoor.addEventListener("click", exitMiniGame);

setEmotion("joy");
updateHud();
