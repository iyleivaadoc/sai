$('#show_password').on('change', function (event) {
    // Si el checkbox esta "checkeado"
    if ($('#show_password').is(':checked')) {
        // Convertimos el input de contraseña a texto.
        $('#OldPassword').get(0).type = 'text';
        // En caso contrario..
    } else {
        // Lo convertimos a contraseña.
        $('#OldPassword').get(0).type = 'password';
    }
});
$('#show_password_new').on('change', function (event) {
    // Si el checkbox esta "checkeado"
    if ($('#show_password_new').is(':checked')) {
        // Convertimos el input de contraseña a texto.
        $('#NewPassword').get(0).type = 'text';
        // En caso contrario..
    } else {
        // Lo convertimos a contraseña.
        $('#NewPassword').get(0).type = 'password';
    }
});
$('#show_password_confirm').on('change', function (event) {
    // Si el checkbox esta "checkeado"
    if ($('#show_password_confirm').is(':checked')) {
        // Convertimos el input de contraseña a texto.
        $('#ConfirmPassword').get(0).type = 'text';
        // En caso contrario..
    } else {
        // Lo convertimos a contraseña.
        $('#ConfirmPassword').get(0).type = 'password';
    }
});
$('#show').on('change', function (event) {
    // Si el checkbox esta "checkeado"
    if ($('#show').is(':checked')) {
        // Convertimos el input de contraseña a texto.
        $('#Password').get(0).type = 'text';
        // En caso contrario..
    } else {
        // Lo convertimos a contraseña.
        $('#Password').get(0).type = 'password';
    }
});