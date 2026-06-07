document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('input[type="number"]').forEach(function (input) {
        input.addEventListener('input', function () {
            if (Number(input.value) < 0) input.value = 0;
        });
    });
});
