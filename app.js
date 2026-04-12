const state = {
    location: "木屋前",
    objective: "发现：空地有一台发光游戏机",
    wood: 0,
    stickers: 0,
    shards: 0,
    bridgeRepaired: false,
    inPixelMode: false,
    completed: false,
    emotion: "joy",
    timeOfDay: "morning",
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
const timeDesc = document.getElementById("timeDesc");

const timeMeta = {
    morning: "清晨适合采集木材和整理木屋周围。",
    noon: "午后光线清楚，适合修桥、摆放花圃和查看河岸。",
    night: "夜晚能看到游戏机和河岸灯的光，适合检查空地布置。",
};

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

function setTimeOfDay(timeKey) {
    state.timeOfDay = timeKey;
    document.body.dataset.time = timeKey;
    timeDesc.textContent = timeMeta[timeKey];
    document.querySelectorAll(".time-button").forEach((button) => {
        button.classList.toggle("active", button.dataset.time === timeKey);
    });
}

function handleWorldAction(action) {
    if (action === "home") {
        state.location = "木屋前";
        state.objective = state.completed ? "发现：贴纸墙新增了一张贴纸" : "发现：空地有一台发光游戏机";
        promptBubble.textContent = state.completed
            ? "贴纸墙已经更新，第一轮闭环完成。"
            : "木屋周围可以继续摆放花圃、路牌和河岸灯。";
        avatarToken.classList.remove("pixel");
        moveAvatar("236px", "250px");
    }

    if (action === "forage") {
        state.location = "旁友森林";
        state.wood = Math.max(state.wood, 1);
        state.shards = Math.max(state.shards, 1);
        state.objective = "发现：木材可用于修桥或制作木屋周围摆件";
        promptBubble.textContent = "拾取到木材和表情碎片。建设清单会显示这些材料能做什么。";
        moveAvatar("58%", "310px");
    }

    if (action === "arcade") {
        if (!state.bridgeRepaired) {
            if (state.wood < 1) {
                state.objective = "发现：木桥缺少 1 个木材";
                promptBubble.textContent = "木桥还不能修，森林里的树枝可以作为木材。";
                updateHud();
                return;
            }

            state.bridgeRepaired = true;
            state.location = "河边木桥";
            state.objective = "发现：河岸和空地已经连通";
            promptBubble.textContent = "木桥补齐。空地游戏机开始发光，玩家可以自己选择是否进入。";
            moveAvatar("50%", "355px");
            updateHud();
            return;
        }

        state.location = "空地游戏机";
        state.inPixelMode = true;
        state.objective = "发现：像素模式需要 3 个贴纸点亮出口";
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
    state.objective = state.stickers >= 3 ? "发现：出口已经点亮" : "发现：贴纸可以装饰木屋贴纸墙";
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
    state.objective = "发现：贴纸墙可以更新";
    promptBubble.textContent = "小游戏完成，回到主世界。游戏机点亮，木屋贴纸墙会新增贴纸。";
    avatarToken.classList.remove("pixel");
    moveAvatar("74%", "456px");
    updateHud();
}

document.querySelectorAll(".mode-button").forEach((button) => {
    button.addEventListener("click", () => setEmotion(button.dataset.mode));
});

document.querySelectorAll(".time-button").forEach((button) => {
    button.addEventListener("click", () => setTimeOfDay(button.dataset.time));
});

document.querySelectorAll(".map-node").forEach((node) => {
    node.addEventListener("click", () => handleWorldAction(node.dataset.action));
});

document.querySelectorAll(".sticker").forEach((button) => {
    button.addEventListener("click", () => collectSticker(button));
});

exitDoor.addEventListener("click", exitMiniGame);

setEmotion("joy");
setTimeOfDay("morning");
updateHud();
