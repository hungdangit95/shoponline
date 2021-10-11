var UserController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
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
            initRoleList();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/User/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtFullName').val(data.FullName);
                    $('#txtUserName').val(data.UserName);
                    $('#txtEmail').val(data.Email);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ckStatus').prop('checked', data.Status === 1);
                    initRoleList(data.Roles);
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
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true)
                        roles.push($(item).prop('value'));
                });
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/User/SaveEntity",
                    data: {
                        Id: id,
                        FullName: fullName,
                        UserName: userName,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function () {
                        shoponline.notify('Save user succesfull', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

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
            shoponline.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/User/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function () {
                        shoponline.notify('Delete successful', 'success');
                        shoponline.stopLoading();
                        loadData();
                    },
                    error: function () {
                        shoponline.notify('Has an error', 'error');
                        shoponline.stopLoading();
                    }
                });
            });
        });

        //$('body').on('click',
        //    'btn-grant',
        //    function(e) {
        //        e.preventDefault();
        //        $.ajax({
        //            type: "Get",
        //            url:"/Admin/Business/Index",
        //            dataType: "json",
        //            beforeSend: function () {
        //                shoponline.startLoading();
        //            },
        //            success: function(response) {

        //            }

        //        });

        //    });
    };

    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);

    }
    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidId').val('');
        initRoleList();
        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ckStatus').prop('checked', true);

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "POST",
            url: "/Admin/User/GetAllPaging",
            data: {
                // categoryId: $('#ddl-category-search').val(),
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
                            FullName: item.FullName,
                            Id: item.Id,
                            UserName: item.UserName,
                            Avatar: item.Avatar === undefined ? '<img src="/admin-side/images/user.png" width=70 />' : '<img src="' + item.Avatar + '" width=70 />',
                            DateCreated: shoponline.dateTimeFormatJson(item.DateModified),
                            Status: shoponline.getStatus(item.Status)
                        });
                    });
                    $("#lbl-total-records").text(response.Data.RowCount);
                    if (render !== undefined) {
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