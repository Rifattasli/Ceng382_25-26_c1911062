document.addEventListener("DOMContentLoaded", () => {
    const loginButton = document.getElementById("login-btn");
    loginButton.addEventListener("click", () => {
        loginButton.textContent = "Entering...";
        loginButton.style.background = "#c9a000";
        
        setTimeout(() => {
            document.body.style.animation = "fadeOut 1.5s forwards";
        }, 1000);
    });
});

const style = document.createElement("style");
style.innerHTML = `
@keyframes fadeOut {
    from { opacity: 1; }
    to { opacity: 0; }
}
`;
document.head.appendChild(style);
