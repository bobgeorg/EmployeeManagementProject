$(document).ready(function () {

    $.ajax({

        url: '/Employee/UpdateCheckBoxList',
        success: function (result) {
            $('#UpdateCheckboxList').html(result);
        }
    });
   

});