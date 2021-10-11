var businessActionController = function () {
    window.index = "";
    this.initialize = function () {
      
        registerEvents();
    }

    function loadData(businessId) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'POST',
            url: '/Admin/BusinessAction/GetAll',
            data: {
                businessId:businessId
            },
            dataType: 'json',
            success: (response) => {
                debugger;
                $.each(response.result,
                    function (i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.id,
                                Name: item.name,

                            });
                        $('#lblTotalRecords').text(response.data.rowCount);
                        if (render !== "") {
                            $('#tbl-content').html(render);
                        }
                    });
            },
            error: function (status) {
                console.log(status);
                shoponline.notify('Không thể load dữ liệu', 'lỗi');
            }
        });

      
    };

    function registerEvents() {

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $('.btn-edit').on('click', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/BusinessAction/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.id);
                    //$('#businessId').val(data.businessId);
                    window.index = data.businessId;
                    $('#txtId').val(data.name);
                    $('#txtName').val(data.description);
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
                var id = $('#hidId').val();
               // var businessId = $('#businessId').val();
             
                var name = $('#txtId').val();
                var description = $('#txtName').val();
                $.ajax({
                    type: "POST",
                    url: "/Admin/BusinessAction/SaveEntity",
                    data: {
                        Id: id,
                        name: name,
                        description: description
                    },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function () {
                        shoponline.notify('Save user succesfull', 'success');
                        $('#modal-add-edit').modal('hide');
                        shoponline.stopLoading();
                        LoadDataPage(window.index);
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

    function LoadDataPage(urlPage) {
        window.location.href = "/Admin/BusinessAction/Index/?businessId=" + urlPage;
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
