document.addEventListener("DOMContentLoaded", function() {
    setInterval(() => {
        var timeNow = Date.now();

        for (element of document.getElementsByClassName("creationTime")) {
            var duration = new Date(timeNow - +element.attributes.value.value);

            var minString = duration.getMinutes().toString().padStart(2, "0")
            var secString = duration.getSeconds().toString().padStart(2, "0")

            element.innerHTML = `${minString}:${secString}`;
        }
    }, 1000);
});