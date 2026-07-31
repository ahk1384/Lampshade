var quill = new Quill('#quill-container', {
    theme: 'snow',
});
quill.format('direction', 'rtl');
quill.format('align', 'right');
var hiddenTextarea = document.getElementById('BodyContent');

if (hiddenTextarea.value) {
    quill.root.innerHTML = hiddenTextarea.value;
}

var form = hiddenTextarea.closest('form');
form.addEventListener('submit', function () {
    hiddenTextarea.value = quill.root.innerHTML;
});