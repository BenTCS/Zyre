const LoadingScreen = document.getElementById('LoadingScreen');
const WelcomeScreen = document.getElementById('WelcomeScreen');
const WelcomeTitle = document.getElementById('WelcomeTitle');
const LoginContainer = document.getElementById('LoginContainer');
const UpdateModal = document.getElementById('UpdateModal');
const BannedModal = document.getElementById('BannedModal');
const DonatorModal = document.getElementById('DonatorModal');
const UpdateButton = document.getElementById('UpdateButton');
const PasswordInput = document.getElementById('PasswordInput');
const PasswordToggle = document.getElementById('PasswordToggle');
const EyeIcon = document.getElementById('EyeIcon');
const LoginBtn = document.getElementById('LoginBtn');

const EonLogo = "https://cdn.eonfn.dev/EonS17.png";

const LoadingMessages = [
    { text: "Starting Up", subtext: "Getting everything ready for you..." },
    { text: "Checking For Updates", subtext: "Making sure you have the latest version..." },
    { text: "Connecting", subtext: "Connecting to the servers..." },
    { text: "Loading", subtext: "Almost there, pulling everything together..." },
    { text: "Preparing", subtext: "Setting up your session..." },
    { text: "Syncing", subtext: "Updating your data..." },
];

const RandomMessage = LoadingMessages[Math.floor(Math.random() * LoadingMessages.length)];
document.querySelector(".loading-text").textContent = RandomMessage.text;
document.querySelector(".loading-subtext").textContent = RandomMessage.subtext;

setTimeout(() => {
    LoadingScreen.classList.add("hidden");
    LoadingScreen.addEventListener("transitionend", () => LoadingScreen.classList.add("done"), { once: true });

    if (!window.chrome || !window.chrome.webview) {
        LoginContainer.classList.add("show");
        return;
    }

    window.chrome.webview.postMessage({ Action: "CheckCredentials" });
}, 2000);

PasswordToggle.addEventListener("click", (Event) => {
    Event.preventDefault();
    Event.stopPropagation();

    const IsPassword = PasswordInput.type === "password";
    PasswordInput.type = IsPassword ? "text" : "password";

    if (IsPassword) {
        EyeIcon.innerHTML = `<path d="M12 7C14.76 7 17 9.24 17 12C17 12.65 16.87 13.26 16.64 13.83L19.56 16.75C21.07 15.49 22.26 13.86 22.99 12C21.26 7.61 16.99 4.5 11.99 4.5C10.59 4.5 9.25 4.75 8.01 5.2L10.17 7.36C10.74 7.13 11.35 7 12 7ZM2 4.27L4.28 6.55L4.74 7.01C3.08 8.3 1.78 10.02 1 12C2.73 16.39 7 19.5 12 19.5C13.55 19.5 15.03 19.2 16.38 18.66L16.8 19.08L19.73 22L21 20.73L3.27 3L2 4.27ZM7.53 9.8L9.08 11.35C9.03 11.56 9 11.78 9 12C9 13.66 10.34 15 12 15C12.22 15 12.44 14.97 12.65 14.92L14.2 16.47C13.53 16.8 12.79 17 12 17C9.24 17 7 14.76 7 12C7 11.21 7.2 10.47 7.53 9.8ZM11.84 9.02L14.99 12.17L15.01 12.01C15.01 10.35 13.67 9.01 12.01 9.01L11.84 9.02Z" fill="white"/>`;
        return;
    }

    EyeIcon.innerHTML = `<path d="M12 4.5C7 4.5 2.73 7.61 1 12C2.73 16.39 7 19.5 12 19.5C17 19.5 21.27 16.39 23 12C21.27 7.61 17 4.5 12 4.5ZM12 17C9.24 17 7 14.76 7 12C7 9.24 9.24 7 12 7C14.76 7 17 9.24 17 12C17 14.76 14.76 17 12 17ZM12 9C10.34 9 9 10.34 9 12C9 13.66 10.34 15 12 15C13.66 15 15 13.66 15 12C15 10.34 13.66 9 12 9Z" fill="white"/>`;
});

function ShowNotification(Title, Message) {
    const Container = document.getElementById("NotificationContainer");
    const Active = Container.querySelectorAll(".notification:not(.removing)");

    if (Active.length >= 5) {
        RemoveNotification(Active[Active.length - 1]);
    }

    const Item = document.createElement("div");
    Item.className = "notification";

    const UrlRegex = /(https?:\/\/[^\s]+)/g;
    const MessageHtml = Message.replace(UrlRegex, `<a href="$1" target="_blank">$1</a>`);

    Item.innerHTML = `
        <div class="notification-content">
            <svg class="notification-icon" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path d="M12 2C6.48 2 2 6.48 2 12C2 17.52 6.48 22 12 22C17.52 22 22 17.52 22 12C22 6.48 17.52 2 12 2ZM13 17H11V15H13V17ZM13 13H11V7H13V13Z"/>
            </svg>
            <div class="notification-body">
                <div class="notification-title">${Title}</div>
                <div class="notification-message">${MessageHtml}</div>
            </div>
        </div>
    `;

    Container.insertBefore(Item, Container.firstChild);
    requestAnimationFrame(() => Item.classList.add("show"));

    Item.dataset.timeoutId = setTimeout(() => RemoveNotification(Item), 5000);
}

function RemoveNotification(Item) {
    if (Item.classList.contains("removing")) return;
    Item.classList.add("removing");
    clearTimeout(parseInt(Item.dataset.timeoutId));
    Item.classList.remove("show");
    Item.classList.add("fade-out");
    setTimeout(() => Item.remove(), 350);
}

function GetDominantColor(ImageUrl, Callback) {
    const Img = new Image();
    Img.crossOrigin = "Anonymous";
    Img.src = ImageUrl;

    Img.onload = function () {
        const Canvas = document.createElement("canvas");
        const Ctx = Canvas.getContext("2d");
        Canvas.width = Img.width;
        Canvas.height = Img.height;
        Ctx.drawImage(Img, 0, 0);

        const Data = Ctx.getImageData(0, 0, Canvas.width, Canvas.height).data;
        let R = 0, G = 0, B = 0, Count = 0;

        for (let i = 0; i < Data.length; i += 4) {
            if (Data[i + 3] <= 128) continue;
            R += Data[i]; G += Data[i + 1]; B += Data[i + 2]; Count++;
        }

        Callback(`rgb(${Math.floor(R / Count)},${Math.floor(G / Count)},${Math.floor(B / Count)})`);
    };
}

function ShowWelcomeScreen(Username, SkinUrl) {
    const Messages = [
        { greeting: `Welcome Back, ${Username || "Player"}!`, subtext: "Great to see you again. Loading your profile..." },
        { greeting: `Hey ${Username || "Player"}!`, subtext: "Good to have you back. Syncing your progress..." },
        { greeting: `What's Up, ${Username || "Player"}?`, subtext: "Glad you're here. Getting your account ready..." },
        { greeting: `Welcome, ${Username || "Player"}!`, subtext: "Happy to see you. Loading your profile..." },
        { greeting: `Let's Go, ${Username || "Player"}!`, subtext: "Ready to get started. Setting up your session..." },
        { greeting: `Back Again, ${Username || "Player"}?`, subtext: "Nice to see you. Loading everything up..." },
    ];

    const Msg = Messages[Math.floor(Math.random() * Messages.length)];
    WelcomeTitle.textContent = Msg.greeting;
    document.querySelector(".welcome-subtitle").textContent = Msg.subtext;

    const Logo = document.getElementById("WelcomeLogo");

    if (!SkinUrl) {
        GetDominantColor(SkinUrl, (Color) => {
            Logo.innerHTML = `
            <div class="glow-bg" style="background:${Color};"></div>
            <img src="${SkinUrl}" class="main-img" alt="Skin" style="width:100%;height:100%;object-fit:cover;">
        `;
        });
        WelcomeScreen.classList.add("show");
        setTimeout(() => {
            WelcomeScreen.classList.add("hidden");
        }, 2000);
        return;
    }

    GetDominantColor(SkinUrl, (Color) => {
        Logo.innerHTML = `
            <div class="glow-bg" style="background:${Color};"></div>
            <img src="${SkinUrl}" class="main-img" alt="Skin" style="width:100%;height:100%;object-fit:cover;">
        `;
    });

    WelcomeScreen.classList.add("show");
    setTimeout(() => {
        WelcomeScreen.classList.add("hidden");
    }, 2000);
}

function ShowUpdateModal(DownloadUrl) {
    UpdateButton.href = DownloadUrl || "#";
    UpdateModal.classList.add("show");
}

function ResetLoginButton() {
    LoginBtn.disabled = false;
    LoginBtn.textContent = "Sign In";
}

function ShowLogin() {
    LoginContainer.style.display = "";
    LoginContainer.classList.add("show");
}

async function HandleLogin() {
    const Email = document.getElementById("EmailInput").value.trim();
    const Password = PasswordInput.value.trim();

    if (!Email || !Password) {
        ShowNotification("Missing Fields", "Please enter both email and password.");
        return;
    }

    if (!Email.includes("@")) {
        ShowNotification("Invalid Email", "Please enter a valid email address.");
        return;
    }

    LoginBtn.disabled = true;
    LoginBtn.innerHTML = `<div class="spinner"></div>`;

    if (!window.chrome || !window.chrome.webview) {
        setTimeout(() => {
            ShowNotification("Connection Error", "Unable to connect to the application.");
            ResetLoginButton();
        }, 1000);
        return;
    }

    window.chrome.webview.postMessage({ Action: "Login", Email, Password });
}

function HandleLoginResponse(Data) {
    if (Data.Status === "Success") {
        LoginContainer.classList.remove("show");
        ShowWelcomeScreen(Data.Username, Data.SkinUrl);
        return;
    }

    if (Data.Status === "OUTDATED") {
        ShowUpdateModal(Data.DownloadUrl);
        ResetLoginButton();
        return;
    }

    if (Data.Status === "Banned") {
        BannedModal.classList.add("show");
        ResetLoginButton();
        return;
    }

    if (Data.Status === "Donator") {
        DonatorModal.classList.add("show");
        ResetLoginButton();
        return;
    }

    const Messages = {
        Deny: "Access Denied.",
        Invalid: "Your email and/or password is invalid.",
        Error: "Unknown error. Please try again.",
    };

    ShowNotification("Login Failed", Messages[Data.Status] || "Login failed. Please try again.");
    ResetLoginButton();
}

function HandleMessage(Event) {
    const Data = Event.data;

    if (Data.Action === "AutoLogin") {
        if (Data.Status === "Success") {
            ShowWelcomeScreen(Data.Username, Data.SkinUrl);
            return;
        }

        if (Data.Status === "OUTDATED") {
            ShowUpdateModal(Data.DownloadUrl);
            return;
        }

        ShowLogin();

        if (Data.Status === "Banned") {
            BannedModal.classList.add("show");
            return;
        }

        if (Data.Status === "Donator") {
            DonatorModal.classList.add("show");
            return;
        }

        return;
    }

    if (Data.Action === "ShowLogin") {
        ShowLogin();
        return;
    }

    if (Data.Action === "LoginResponse") {
        HandleLoginResponse(Data);
        return;
    }
}

if (window.chrome && window.chrome.webview) {
    window.chrome.webview.addEventListener("message", HandleMessage);
}

window.addEventListener("message", HandleMessage);

document.getElementById("EmailInput").addEventListener("keypress", (Event) => {
    if (Event.key !== "Enter") return;
    Event.preventDefault();
    HandleLogin();
});

document.getElementById("PasswordInput").addEventListener("keypress", (Event) => {
    if (Event.key !== "Enter") return;
    Event.preventDefault();
    HandleLogin();
});