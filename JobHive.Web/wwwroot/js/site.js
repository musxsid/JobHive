// JobHive Interactive Client Scripts

document.addEventListener("DOMContentLoaded", function () {
    // Enable Bootstrap tooltips if any
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // File input name preview
    const fileInputs = document.querySelectorAll('input[type="file"]');
    fileInputs.forEach(input => {
        input.addEventListener('change', function (e) {
            if (this.files && this.files[0]) {
                const fileName = this.files[0].name;
                const parentBox = this.closest('.border') || this.parentElement;
                const existingLabel = parentBox.querySelector('.file-selected-label');
                if (existingLabel) {
                    existingLabel.textContent = `Selected: ${fileName}`;
                }
            }
        });
    });
});
