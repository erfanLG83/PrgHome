const profileToggle = document.getElementById('profile-toggle');
const profileBox = document.getElementById('profile-box');
const profileClose = () => {
    profileToggle.classList.remove('fa-caret-up');
    profileToggle.classList.add('fa-caret-down');
    profileBox.classList.remove('animate__fadeInDown');
    profileBox.classList.add('animate__fadeOutDown');
}
profileToggle.addEventListener('click', () => {
    if (profileToggle.classList.contains('fa-caret-down')) {
        profileToggle.classList.remove('fa-caret-down');
        profileToggle.classList.add('fa-caret-up');
        profileBox.classList.remove("animate__fadeOutDown");
        profileBox.classList.add('animate__fadeInDown');
    } else {
        profileClose();
    }
})
const liveImage = (inputId, imgId,toggleId) => {
    let inputElem = document.querySelector("#" + inputId);
    let imgElem = document.querySelector("#" + imgId);
    let toggleElem = document.querySelector("#" + toggleId);
    inputElem.addEventListener('change',() => {
        if (inputElem.files.length > 0) {
            toggleElem.classList.remove('hidden');
            imgElem.src = URL.createObjectURL(inputElem.files[0]);
            imgElem.classList.add('animate__fadein');
            setTimeout(() => {
                imgElem.classList.remove('animate__fadein');
            }, 1000)
            imgElem.setAttribute('data-last-img', 'false');
            document.querySelector('#LastImageDeleted').value = "true";
        }
    })
    toggleElem.addEventListener('click', () => {
        if (imgElem.getAttribute('data-last-img') === "true") {
            imgElem.setAttribute('data-last-img', 'false');
            document.querySelector('#LastImageDeleted').value = "true";
        }
        inputElem.value = null;
        toggleElem.classList.add('hidden');
        imgElem.classList.add('animate__fadeout');
        setTimeout(() => {
            imgElem.src = imgElem.getAttribute('data-default-src');
            imgElem.classList.replace('animate__fadeout', 'animate__fadein');
        }, 500);
    });
}