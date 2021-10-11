var cart = {
    init: function () {
        cart.registerEvent();
    },
    registerEvent: function () {
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Số điện thoại được yêu cầu",
                    number: "Số điện thoại phải là số."
                }
            }
        });
     
        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });
        
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }
        });

        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() === 'NL') {
                $('.boxContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() === 'ATM_ONLINE') {
                $('.boxContent').hide();
                $('#bankContent').show();
            }
            else if ($(this).val() === 'CASH') {
                $('.boxContent').hide();
                $('#codContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },

    createOrder: function () {
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            //CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            TypePayment: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[groupname="bankcode"]:checked').prop('id'),
            Status: false
        }
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/Cart/Checkout',
            data: {
                CustomerName: $('#txtName').val(),
                CustomerAddress: $('#txtAddress').val(),
                //CustomerEmail: $('#txtEmail').val(),
                CustomerMobile: $('#txtPhone').val(),
                CustomerMessage: $('#txtMessage').val(),
                TypePayment: $('input[name="paymentMethod"]:checked').val(),
                BankCode: $('input[groupname="bankcode"]:checked').prop('id')
            },
            success: function (response) {
                if (response.status) {
                    window.location.replace(response.url);
                    return;
                }
                shoponline.notify(response.message, 'warning');
            }
        });
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    }
}
cart.init();