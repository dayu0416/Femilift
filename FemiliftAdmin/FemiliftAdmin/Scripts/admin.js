$(function () {
    // SortableJS - drag & drop reorder
    var branchList = document.getElementById('branch-sortable-list');
    if (branchList) {
        new Sortable(branchList, {
            handle: '.drag-handle',
            animation: 150,
            ghostClass: 'sortable-ghost',
            onEnd: function () {
                var ids = [];
                $(branchList).find('.branch-admin-item').each(function () {
                    ids.push($(this).data('id'));
                });
                $.ajax({
                    url: '/Admin/Branches/Reorder',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(ids),
                    success: function (res) {
                        if (!res.success) alert('排序儲存失敗');
                    },
                    error: function () {
                        alert('排序儲存失敗');
                    }
                });
            }
        });
    }

    // Toggle visibility
    $(document).on('click', '.btn-toggle', function () {
        var btn = $(this);
        var id = btn.data('id');
        $.ajax({
            url: '/Admin/Branches/Toggle/' + id,
            type: 'POST',
            success: function (res) {
                if (res.success) {
                    if (res.isVisible) {
                        btn.removeClass('hidden').addClass('visible').text('顯示');
                    } else {
                        btn.removeClass('visible').addClass('hidden').text('隱藏');
                    }
                }
            },
            error: function () {
                alert('操作失敗');
            }
        });
    });

    // Delete confirmation
    $(document).on('click', '.btn-delete', function (e) {
        if (!confirm('確定要刪除此院所嗎？此操作無法復原。')) {
            e.preventDefault();
        }
    });

    // Image preview on file select
    $('#ImageFile').on('change', function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#image-preview').attr('src', e.target.result).show();
            };
            reader.readAsDataURL(file);
        }
    });

    // Remove image checkbox
    $('#RemoveImage').on('change', function () {
        if ($(this).is(':checked')) {
            $('#image-preview').hide();
            $('#ImageFile').val('');
        } else {
            var existing = $('#image-preview').data('existing');
            if (existing) {
                $('#image-preview').attr('src', '/' + existing).show();
            }
        }
    });
});
