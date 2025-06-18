document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("login-form");

    form.addEventListener("submit", function (e) {
        e.preventDefault(); // предотвращаем обычную отправку формы

        hideFormAlert();

        const formData = new FormData(form);

        // Пример клиентской валидации
        if (formData.get("password").length === 0) {
            showFormAlert("Введите пароль!");
            return;
        }

        fetch("https://localhost:5234/api/Auth", {
            method: "POST",
            body: formData
        })
            .then(async res => {
                const data = await res.json();
                console.log(data);

                if (!res.ok)
                    throw new Error(data.error);

                document.cookie = `auth_token=${data.token}; path=/; max-age=3600`;
            })
            .then(data => {
                hideLoginForm();
                loadUnityApp();
            })
            .catch(err => {
                console.error(err);
                showFormAlert(err.message, 'loginForm');
            });
    });
});

function showLoginForm(login = null) {
    hideRegistrationForm();
    const loginWindow = document.getElementById('login-window');
    loginWindow.style.visibility = "visible";
    if (login) {
        const loginFormField = loginWindow.querySelector('#login-email');
        loginFormField.value = login;
    }

}

function hideLoginForm() {
    const loginWindow = document.getElementById('login-window');
    loginWindow.style.visibility = "hidden";
    hideFormAlert('loginForm');
}