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
