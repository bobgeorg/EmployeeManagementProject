$(document).ready(function () {
    $.ajax({

        url: '/Employee/AddNonExistingSkill',
        success: function (result) {
            $('#AddNonExistingSkill').html(result);
        }
    }); 
});