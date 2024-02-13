$(document).ready(function () {
    $('#ChooseImage').change(function (e) {
        var url = $('#ChooseImage').val()
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase()
        if (ChooseImage.files && ChooseImage.files[0] && (ext == "gif" || ext == "jpg" || ext == "jfif" || ext == "png" || ext == "bmp" || ext == "webp")) {
            var reader = new FileReader()
            reader.onload = function () {
                var output = document.getElementById('PreviewImage');
                output.src = reader.result
            }
            reader.readAsDataURL(e.target.files[0])
        }
    });
});