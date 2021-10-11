var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtUserName: {
                    required: true
                },
                txtPassword: {
                    required: true
                }
            },
            messages: {
                txtUserName:{
                    required: "Bạn phải nhâp email"
                },
                txtPassword: {
                    required: "Bạn phải nhập mật khẩu"
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                var email = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(email, password);
            }
        });
    }

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                Email: user,
                Password: pass
            },
            dateType: 'json',
            url: '/Admin/Login/Authen',
            success: function (res) {

                if (res.Success===true) {
                    window.location.href = "/Admin/Home/Index";
                    shoponline.notify('Đăng nhập thành công', 'success');
                }
                else {
                    window.location.href = "/admin-login";
                    shoponline.notify('Tên đăng nhập hoặc mật khẩu không đúng', 'error');
                }
            }
        })
    }
}