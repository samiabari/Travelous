function Validate(ctl, event) {
    event.preventDefault();
    swal({
        title: "Do you want to Update it?",
        text: "Please check Information before Submiting!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Update",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {

                $("#UpdateForm").submit();

            } else {

                event.preventDefault();
            }
        });
}