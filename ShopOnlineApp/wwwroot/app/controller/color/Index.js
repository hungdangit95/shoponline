var ColorController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtAliasM: { required: true }
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#ddl-show-page").on('change', function () {
            shoponline.configs.pageSize = $(this).val();
            shoponline.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            debugger;
            $.ajax({
                type: "GET",
                url: "/Admin/Color/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    $('#txtCode').val(data.Code);
                    $('#modal-add-edit').modal('show');
                    shoponline.stopLoading();
                },
                error: function () {
                    shoponline.notify('Có lỗi xảy ra', 'error');
                    shoponline.stopLoading();
                }
            });
        });

        $('#btnSaveM').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var code = $('#txtCode').val();
                debugger;
                $.ajax({
                    type: "POST",
                    url: "/Admin/Color/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Code: code
                    },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function () {
                        shoponline.notify('Update page successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        shoponline.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        shoponline.notify('Have an error in progress', 'error');
                        shoponline.stopLoading();
                    }
                });
                return false;
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            shoponline.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Color/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function () {
                        shoponline.notify('Delete page successful', 'success');
                        shoponline.stopLoading();
                        loadData();
                    },
                    error: function () {
                        shoponline.notify('Have an error in progress', 'error');
                        shoponline.stopLoading();
                    }
                });
            });
        });
    };

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtCode').val('');
    }

  

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/Color/GetAllPaging",
            data: {
                searchText: $('#txt-search-keyword').val(),
                page: shoponline.configs.pageIndex,
                pageSize: shoponline.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                shoponline.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.Data.RowCount > 0) {
                    $.each(response.Data.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Code: item.Code,
                            Id: item.Id
                        });
                    });
                    $("#lbl-total-records").text(response.Data.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);
                    }
                    wrapPaging(response.Data.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                }
                else {
                    $('#tbl-content').html('');
                }
                shoponline.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / shoponline.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                shoponline.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}