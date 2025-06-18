function loadUnityApp() {
    container.style.display = "block";
    window.scrollTo({top: 0, behavior: 'instant'});

    const script = document.createElement("script");
    script.src = loaderUrl;
    script.onload = () => {
        createUnityInstance(canvas, config, (progress) => {
            progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {
            loadingBar.style.display = "none";
        }).catch((message) => {
            alert(message);
        });
    };
    document.body.appendChild(script);
}

function showFormAlert(message, form = 'loginForm') {
    const alertFormId = form === 'loginForm' ? '#login-window' : '#registration-window';
    const formAlert = document.querySelector(`${alertFormId} .form-alert`);
    formAlert.style.visibility = "visible";
    formAlert.innerHTML = message;
}

function hideFormAlert(form = 'loginForm' || 'registrationForm') {
    const alertFormId = form === 'loginForm' ? '#login-window' : '#registration-window';
    const formAlert = document.querySelector(`${alertFormId} .form-alert`);
    formAlert.style.visibility = "hidden";
}