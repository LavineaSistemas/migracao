function Msgtoast(msg, tipo) {
    const Toast = Swal.mixin({
        toast: true,
        position: "top-center",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    
    if (tipo == 1) {
        Toast.fire({
            icon: "success",
            title: msg,
        });
    }

    if (tipo == 2) {
        Toast.fire({
            icon: "error",
            title: msg,
        });
    }
}