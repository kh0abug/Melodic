
$(document).ready(function () {
    $('#pagination').jqPaginator({
        totalPages: @Model.TotalPage,
        visiblePages: 2,
        currentPage: @Model.PageIndex,
        prev: '<li class="page-item"><a class="page-link" href="javascript:;">‹</a></li>',
        next: '<li class="page-item"><a class="page-link" href="javascript:;">›</a></li>',
        page: '<li class="page-item"><a class="page-link" href="javascript:;">{{page}}</a></li>',
        onPageChange: function (num) {
            if (this.currentPage != num) {
                $('#page-index').val(num),
                    $('#page-form').submit()
            }
        }
    });
})
