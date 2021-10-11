var businessController = function () {
    //var business = new businessActionController();
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function loadData(isPageChanged) {
      
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'POST',
            url: '/Admin/Business/GetAllPaging',
            data: {
                searchText: $('#txt-search-keyword').val(),
                pageIndex: shoponline.configs.pageIndex,
                pageSize: shoponline.configs.pageSize
            },
            dataType: 'json',
            success: (response) => {
                $.each(response.Data.Items,
                    function (i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.Id,
                                Name: item.Name
                            });
                        $('#lblTotalRecords').text(response.Data.RowCount);
                        if (render !== "") {
                            $('#tbl-content').html(render);
                        }
                        wrapPaging(response.Data.RowCount, function () {
                            loadData();
                        }, isPageChanged);
                    });
            },
            error: function (status) {
                console.log(status);
                shoponline.notify('Không thể load dữ liệu', 'lỗi');
            }
        });

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

    };

    function registerEvents() {

        $('#txt-search-keyword').keypress(function (e) {
         if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Business/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.id);
                    $('#txtId').val(data.id);
                    $('#txtName').val(data.name);
                    disableFieldEdit(true);
                    $('#modal-add-edit').modal('show');
                    shoponline.stopLoading();
                },
                error: function () {
                    shoponline.notify('Có lỗi xảy ra', 'error');
                    shoponline.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#txtId').val();
                var name = $('#txtName').val();
                $.ajax({
                    type: "POST",
                    url: "/Admin/Business/SaveEntity",
                    data: {
                        Id: id,
                        name: name
                    },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function () {
                        shoponline.notify('Save user succesful', 'success');
                        $('#modal-add-edit').modal('hide');
                        shoponline.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        shoponline.notify('Has an error', 'error');
                        shoponline.stopLoading();
                    }
                });
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteBusiness(that);
        });
    }

    function deleteBusiness(id) {
        shoponline.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Business/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    shoponline.notify('Delete successful', 'success');
                    shoponline.stopLoading();
                    loadData();
                },
                error: function (status) {
                    shoponline.notify('Has an error in delete progress', 'error');
                    shoponline.stopLoading();
                }
            });
        });
    }

    function disableFieldEdit(disabled) {
        $('#txtId').prop('disabled', disabled);
    }



}
