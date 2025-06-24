document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("registration-form");

    form.addEventListener("submit", function (e) {
        e.preventDefault(); // предотвращаем обычную отправку формы

        hideFormAlert();

        const formData = new FormData(form);

        // Пример клиентской валидации
        if (formData.get("password") !== formData.get("password2")) {
            showFormAlert("Пароли не совпадают!");
            return;
        }

        deleteCookie("auth_token");

        fetch("https://localhost:5234/api/Registration", {
            method: "POST",
            body: formData
        })
            .then(async res => {
                const data = await res.json();
                console.log(data);

                if (!res.ok) {
                    if (data.errors)
                        throw new Error("Все поля обязательны для заполнения");
                    else
                        throw new Error(data.error);
                }
            })
            .then(data => {
                showLoginForm(formData.get("email"));
            })
            .catch(err => {
                console.error(err);
                showFormAlert(err.message, 'registrationForm');
            });
    });
});

function showRegistrationForm() {
    hideLoginForm();
    const registrationWindow = document.getElementById('registration-window');
    registrationWindow.style.visibility = "visible";
}

function hideRegistrationForm() {
    const registrationWindow = document.getElementById('registration-window');
    registrationWindow.style.visibility = "hidden";
    hideFormAlert('registrationForm');
}
