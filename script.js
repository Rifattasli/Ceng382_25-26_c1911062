document.addEventListener("DOMContentLoaded", () => {
    
    const loginButton = document.getElementById("login-btn");
    const usernameInput = document.getElementById("username");
    const passwordInput = document.getElementById("password");
    const messageDiv = document.getElementById("message");

    const loginAttempts = [];
    const validCredentials = { username: "admin", password: "admin" };

    if (loginButton) {
        loginButton.addEventListener("click", () => {
            const username = usernameInput.value.trim();
            const password = passwordInput.value.trim();

            if (username === validCredentials.username && password === validCredentials.password) {
                loginAttempts.push({ username, password });
                console.log("Login Successful!", loginAttempts);
                messageDiv.textContent = "✅ Login successful! Redirecting...";
                messageDiv.className = "success";
                
                setTimeout(() => {
                    window.location.href = "table.html"; // Redirect to class table page
                }, 1500);
            } else {
                messageDiv.textContent = "❌ Incorrect Username or Password!";
                messageDiv.className = "error";
            }
        });
    }

    // clock
    function updateClock() {
        const clockElement = document.getElementById("clock");
        if (!clockElement) return;
        const now = new Date();
        const hours = now.getHours().toString().padStart(2, "0");
        const minutes = now.getMinutes().toString().padStart(2, "0");
        const seconds = now.getSeconds().toString().padStart(2, "0");

        clockElement.textContent = `🕰️ ${hours}:${minutes}:${seconds}`;
    }

    setInterval(updateClock, 1000);
    updateClock(); 

    // H key
    document.addEventListener("keydown", (event) => {
        if (event.key.toLowerCase() === "h") {
            document.querySelectorAll(".input-group, button").forEach(el => {
                el.style.display = el.style.display === "none" ? "block" : "none";
            });
        }
    });

    // table
    const classForm = document.getElementById("class-form");
    const classTable = document.getElementById("class-table")?.querySelector("tbody");

    if (classForm && classTable) {
        classForm.addEventListener("submit", (event) => {
            event.preventDefault(); // Prevent form reload

            
            const className = document.getElementById("class-name").value.trim();
            const numPeople = document.getElementById("num-people").value.trim();
            const description = document.getElementById("description").value.trim();

            if (className === "" || numPeople === "" || description === "") {
                alert("⚠️ Please fill out all fields!");
                return;
            }

            
            const row = document.createElement("tr");
            row.innerHTML = `
                <td class="editable">${className}</td>
                <td class="editable">${numPeople}</td>
                <td class="editable">${description}</td>
                <td><button class="delete-btn">❌ Remove</button></td>
            `;

            
            classTable.appendChild(row);

            
            row.addEventListener("click", () => {
                row.classList.toggle("highlight");
            });

            
            row.querySelectorAll(".editable").forEach(cell => {
                cell.addEventListener("dblclick", () => {
                    const currentValue = cell.textContent;
                    const input = document.createElement("input");
                    input.type = "text";
                    input.value = currentValue;
                    cell.innerHTML = "";
                    cell.appendChild(input);
                    input.focus();

                    input.addEventListener("blur", () => {
                        cell.textContent = input.value;
                    });
                });
            });

            
            row.querySelector(".delete-btn").addEventListener("click", () => {
                row.remove();
            });

        
            classForm.reset();
        });
    }

    
    document.querySelectorAll("input, textarea").forEach(inputField => {
        inputField.addEventListener("focus", () => {
            inputField.style.boxShadow = "0 0 10px gold";
        });

        inputField.addEventListener("blur", () => {
            inputField.style.boxShadow = "none";
        });
    });
});