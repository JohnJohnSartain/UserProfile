let eventMethod = window.addEventListener
    ? "addEventListener"
    : "attachEvent";

let eventer = window[eventMethod];

let messageEvent = eventMethod === "attachEvent"
    ? "onmessage"
    : "message";

eventer(messageEvent, (e) => {
    let data = e.data;

    if (data !== undefined && data.token !== null && data.token !== undefined)
        if (data.token.length > 75 && data.token.length < 100000) {
            localStorage.setItem('Token', data.token);
            window.location.href = '/';
        }
});