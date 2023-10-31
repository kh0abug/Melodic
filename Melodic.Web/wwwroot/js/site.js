// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('button.delete-button').on('click', function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'question',
            showDenyButton: true,
            allowOutsideClick: false,
            confirmButtonText: 'Yes',
            denyButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                $('#current-id').val($(this).val())
                $('#page-delete').submit();
            } else if (result.isDenied) {
                Swal.fire('Delete Fail', '', 'error')
            }
        });
    });
});