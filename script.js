document.addEventListener("DOMContentLoaded", () => {
    const loginButton = document.getElementById("login-btn");
    const usernameInput = document.querySelector("input[type='text']");
    const passwordInput = document.querySelector("input[type='password']");
    const messageDiv = document.createElement("div");
    messageDiv.id = "message";
    document.querySelector(".login-container").appendChild(messageDiv);

    const loginAttempts = [];
    const formElements = document.querySelectorAll(".input-group, button"); // Select all inputs & button
    let isHidden = false; // Track form visibility state

    loginButton.addEventListener("click", () => {
        const username = usernameInput.value.trim();
        const password = passwordInput.value.trim();

        if (username === "" || password === "") {
            showMessage("⚠️ Please enter both username and password!", "error");
            return;
        }

        loginAttempts.push({ username, password });
        console.log("Login Attempts:", loginAttempts);
        showMessage(`✅ Welcome, ${username}!`, "success");

        loginButton.textContent = "Entering...";
        loginButton.style.background = "#c9a000";

        setTimeout(() => {
            document.body.style.animation = "fadeOut 1.5s forwards";
        }, 1000);
    });

    function showMessage(msg, type) {
        messageDiv.textContent = msg;
        messageDiv.className = type;
    }

    //  Live Clock Function
    function updateClock() {
        const clockElement = document.getElementById("clock");
        const now = new Date();
        const hours = now.getHours().toString().padStart(2, "0");
        const minutes = now.getMinutes().toString().padStart(2, "0");
        const seconds = now.getSeconds().toString().padStart(2, "0");

        clockElement.textContent = `🕰️ ${hours}:${minutes}:${seconds}`;
    }

    setInterval(updateClock, 1000);
    updateClock(); // Run once immediately

    //  Toggle form visibility on 'H' key press
    document.addEventListener("keydown", (event) => {
        if (event.key.toLowerCase() === "h") {
            isHidden = !isHidden;
            formElements.forEach(el => {
                el.style.display = isHidden ? "none" : "block";
            });
        }
    });
});