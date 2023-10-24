// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('button.deactivate').on('click', function () {
        Swal.fire({
            title: 'Hủy kích hoạt',
            text: "Bạn chắc chắn muốn hủy kích hoạt",
            icon: 'question',
            showDenyButton: true,
            allowOutsideClick: false,
            confirmButtonText: 'Có',
            denyButtonText: 'Không',
        }).then((result) => {
            if (result.isConfirmed) {
                $('#current-id').val($(this).val())
                $('#action').val('deactivate')
                $('#page-delete').submit();
            } else if (result.isDenied) {
                Swal.fire('Thay đổi không được lưu', '', 'info')
            }
        });
    });
});