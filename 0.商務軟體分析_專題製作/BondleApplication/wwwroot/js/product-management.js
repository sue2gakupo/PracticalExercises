class ProductManager {
    constructor() {
        this.currentProductId = null;
        this.isMainProductSaved = false;
        this.init();
    }

    init() {
        this.bindEvents();
        this.initializeToastr();
    }

    initializeToastr() {
        // 配置 Toastr 通知
        if (typeof toastr !== 'undefined') {
            toastr.options = {
                closeButton: true,
                debug: false,
                newestOnTop: true,
                progressBar: true,
                positionClass: "toast-top-right",
                preventDuplicates: false,
                onclick: null,
                showDuration: "300",
                hideDuration: "1000",
                timeOut: "5000",
                extendedTimeOut: "1000",
                showEasing: "swing",
                hideEasing: "linear",
                showMethod: "fadeIn",
                hideMethod: "fadeOut"
            };
        }
    }

    bindEvents() {
        // 主要商品表單提交
        $(document).on('submit', '#mainProductForm', (e) => {
            this.handleMainProductSubmit(e);
        });

        // AJAX表單提交
        $(document).on('submit', '.ajax-form', (e) => {
            this.handleAjaxFormSubmit(e);
        });

        // Tab 切換事件
        $(document).on('shown.bs.tab', 'button[data-bs-toggle="tab"]', (e) => {
            this.handleTabSwitch(e);
        });

        // 圖片上傳
        $(document).on('submit', '#imageUploadForm', (e) => {
            this.handleImageUpload(e);
        });

        // 設為主圖片
        $(document).on('click', '.set-main-btn', (e) => {
            this.setMainImage(e);
        });

        // 刪除圖片
        $(document).on('click', '.delete-image-btn', (e) => {
            this.deleteImage(e);
        });

        // 新增商品變化
        $(document).on('click', '#addVariationBtn', (e) => {
            this.addVariation(e);
        });

        // 刪除商品變化
        $(document).on('click', '.remove-variation', (e) => {
            this.removeVariation(e);
        });

        // 商品變化表單提交
        $(document).on('submit', '.variation-form', (e) => {
            this.handleVariationSubmit(e);
        });

        // 文件上傳預覽
        $(document).on('change', 'input[type="file"]', (e) => {
            this.previewImages(e);
        });
    }

    handleMainProductSubmit(e) {
        e.preventDefault();
        const form = $(e.target);
        const formData = form.serialize();

        this.showLoading(form);

        $.post(form.attr('action'), formData)
            .done((response) => {
                this.hideLoading(form);

                if (response.success !== false) {
                    // 假設回應包含商品ID或重定向
                    this.currentProductId = response.id || response.productId || this.extractProductIdFromResponse(response);
                    this.isMainProductSaved = true;

                    this.showSuccess('商品基本資訊已儲存');
                    this.enableSubForms();
                    this.loadSubForms();
                } else {
                    this.showError(response.message || '儲存失敗');
                }
            })
            .fail((xhr) => {
                this.hideLoading(form);
                this.handleAjaxError(xhr);
            });
    }

    handleAjaxFormSubmit(e) {
        e.preventDefault();
        const form = $(e.target);
        const formData = form.serialize();
        const url = form.attr('action') || form.data('action');

        if (!url) {
            this.showError('表單設定錯誤：缺少提交URL');
            return;
        }

        this.showLoading(form);

        $.post(url, formData)
            .done((response) => {
                this.hideLoading(form);

                if (response.success) {
                    this.showSuccess(response.message);

                    // 如果是新增操作，重新載入相關區塊
                    if (response.isNew) {
                        this.reloadPartialContainer(form);
                    }
                } else {
                    const errorMessage = this.formatErrors(response.errors) || response.message || '操作失敗';
                    this.showError(errorMessage);
                }
            })
            .fail((xhr) => {
                this.hideLoading(form);
                this.handleAjaxError(xhr);
            });
    }

    handleTabSwitch(e) {
        if (!this.isMainProductSaved || !this.currentProductId) {
            return;
        }

        const target = $(e.target).data('bs-target');
        const container = $(target);
        const controller = container.data('controller');
        const action = container.data('action');

        if (controller && action && container.find('.tab-content-container').children().length <= 1) {
            this.loadPartialContent(container, controller, action);
        }
    }

    handleImageUpload(e) {
        e.preventDefault();
        const form = $(e.target);
        const fileInput = form.find('input[type="file"]')[0];

        if (!fileInput.files.length) {
            this.showError('請選擇要上傳的圖片');
            return;
        }

        // 檢查檔案大小
        const maxSize = parseInt(fileInput.dataset.maxSize) || 5242880; // 5MB
        const file = fileInput.files[0];

        if (file.size > maxSize) {
            this.showError(`檔案大小不能超過 ${(maxSize / 1024 / 1024).toFixed(1)}MB`);
            return;
        }

        const formData = new FormData(form[0]);
        this.showLoading(form);

        $.ajax({
            url: '/ProductImage/Upload',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: (response) => {
                this.hideLoading(form);

                if (response.success) {
                    this.showSuccess(response.message);
                    this.reloadImageContainer();
                    form[0].reset();
                    $('#imagePreview').empty();
                } else {
                    this.showError(response.message);
                }
            },
            error: (xhr) => {
                this.hideLoading(form);
                this.handleAjaxError(xhr);
            }
        });
    }

    setMainImage(e) {
        const imageId = $(e.target).data('image-id');

        $.post('/ProductImage/SetAsMain', { id: imageId })
            .done((response) => {
                if (response.success) {
                    this.showSuccess(response.message);
                    this.reloadImageContainer();
                } else {
                    this.showError(response.message);
                }
            })
            .fail((xhr) => {
                this.handleAjaxError(xhr);
            });
    }

    deleteImage(e) {
        if (!confirm('確定要刪除此圖片嗎？')) {
            return;
        }

        const imageId = $(e.target).data('image-id');

        $.post('/ProductImage/Delete', { id: imageId })
            .done((response) => {
                if (response.success) {
                    this.showSuccess(response.message);
                    $(`[data-image-id="${imageId}"]`).fadeOut(300, function () {
                        $(this).remove();
                    });
                } else {
                    this.showError(response.message);
                }
            })
            .fail((xhr) => {
                this.handleAjaxError(xhr);
            });
    }

    addVariation(e) {
        const productId = $(e.target).data('product-id');

        $.get(`/ProductVariation/GetNewVariationForm/${productId}`)
            .done((data) => {
                $('#variationsContainer .alert').remove();
                $('#variationsContainer').append(data);

                // 滾動到新增的變化項目
                const newItem = $('#variationsContainer .variation-item').last();
                $('html, body').animate({
                    scrollTop: newItem.offset().top - 100
                }, 500);
            })
            .fail((xhr) => {
                this.handleAjaxError(xhr);
            });
    }

    removeVariation(e) {
        const container = $(e.target).closest('.variation-item');
        const variationId = container.find('[name$=".Id"]').val();

        if (variationId && variationId > 0) {
            if (!confirm('確定要刪除此商品變化嗎？')) {
                return;
            }

            $.post('/ProductVariation/Delete', { id: variationId })
                .done((response) => {
                    if (response.success) {
                        container.fadeOut(300, function () {
                            $(this).remove();
                        });
                        this.showSuccess(response.message);
                    } else {
                        this.showError(response.message);
                    }
                })
                .fail((xhr) => {
                    this.handleAjaxError(xhr);
                });
        } else {
            container.fadeOut(300, function () {
                $(this).remove();
            });
        }
    }

    handleVariationSubmit(e) {
        e.preventDefault();
        const form = $(e.target);
        const id = form.find('[name="Id"]').val();
        const url = id && id > 0 ? '/ProductVariation/Update' : '/ProductVariation/Create';

        this.showLoading(form);

        $.post(url, form.serialize())
            .done((response) => {
                this.hideLoading(form);

                if (response.success) {
                    this.showSuccess(response.message);

                    if (!id || id <= 0) {
                        // 重新載入變化列表以獲得新的ID
                        this.reloadVariationsContainer();
                    }
                } else {
                    const errorMessage = this.formatErrors(response.errors) || '儲存失敗';
                    this.showError(errorMessage);
                }
            })
            .fail((xhr) => {
                this.hideLoading(form);
                this.handleAjaxError(xhr);
            });
    }

    previewImages(e) {
        const files = e.target.files;
        const previewContainer = $('#imagePreview');
        previewContainer.empty();

        if (files.length === 0) return;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];

            if (!file.type.startsWith('image/')) {
                continue;
            }

            const reader = new FileReader();
            reader.onload = function (event) {
                const img = $(`
                    <div class="col-md-3 mb-2">
                        <div class="card">
                            <img src="${event.target.result}" class="card-img-top" 
                                 style="height: 150px; object-fit: cover;" alt="預覽">
                            <div class="card-body p-2">
                                <small class="text-muted">${file.name}</small>
                            </div>
                        </div>
                    </div>
                `);
                previewContainer.append(img);
            };
            reader.readAsDataURL(file);
        }
    }

    loadSubForms() {
        if (!this.currentProductId) return;

        const loadPromises = [
            this.loadPartialContent('#digitalProductContainer', 'DigitalProduct', 'GetByProductId'),
            this.loadPartialContent('#physicalProductContainer', 'PhysicalProduct', 'GetByProductId'),
            this.loadPartialContent('#variationsContainer', 'ProductVariation', 'GetByProductId'),
            this.loadPartialContent('#seriesContainer', 'ProSeries', 'GetByProductId'),
            this.loadPartialContent('#productImagesContainer', 'ProductImage', 'GetByProductId')
        ];

        Promise.allSettled(loadPromises).then(() => {
            console.log('所有子表單載入完成');
        });
    }

    loadPartialContent(container, controller, action) {
        const $container = $(container);
        const url = `/${controller}/${action}/${this.currentProductId}`;

        return new Promise((resolve, reject) => {
            $container.load(url, (response, status, xhr) => {
                if (status === 'error') {
                    $container.html('<div class="alert alert-danger">載入失敗，請重新整理頁面</div>');
                    reject(xhr);
                } else {
                    resolve(response);
                }
            });
        });
    }

    enableSubForms() {
        $('.tab-pane .alert-info').fadeOut(300);
        $('button[data-bs-toggle="tab"]').removeClass('disabled');
    }

    reloadImageContainer() {
        if (this.currentProductId) {
            this.loadPartialContent('#productImagesContainer', 'ProductImage', 'GetByProductId');
        }
    }

    reloadVariationsContainer() {
        if (this.currentProductId) {
            this.loadPartialContent('#variationsContainer', 'ProductVariation', 'GetByProductId');
        }
    }

    reloadPartialContainer(form) {
        const container = form.closest('.tab-pane');
        const controller = container.data('controller');
        const action = container.data('action');

        if (controller && action) {
            this.loadPartialContent(`#${container.attr('id')}Container`, controller, action);
        }
    }

    showLoading(element) {
        const $element = $(element);
        const submitBtn = $element.find('button[type="submit"]');

        submitBtn.prop('disabled', true)
            .data('original-text', submitBtn.text())
            .html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>處理中...');
    }

    hideLoading(element) {
        const $element = $(element);
        const submitBtn = $element.find('button[type="submit"]');

        submitBtn.prop('disabled', false)
            .text(submitBtn.data('original-text') || '提交');
    }

    showSuccess(message) {
        if (typeof toastr !== 'undefined') {
            toastr.success(message);
        } else {
            alert(message);
        }
    }

    showError(message) {
        if (typeof toastr !== 'undefined') {
            toastr.error(message);
        } else {
            alert(message);
        }
    }

    formatErrors(errors) {
        if (!errors || !Array.isArray(errors)) {
            return null;
        }
        return errors.join('<br>');
    }

    handleAjaxError(xhr) {
        let message = '發生錯誤，請稍後再試';

        if (xhr.status === 400) {
            message = '請求資料有誤，請檢查輸入內容';
        } else if (xhr.status === 401) {
            message = '未授權存取，請重新登入';
        } else if (xhr.status === 403) {
            message = '沒有權限執行此操作';
        } else if (xhr.status === 404) {
            message = '找不到指定的資源';
        } else if (xhr.status === 500) {
            message = '伺服器內部錯誤';
        }

        this.showError(message);
    }

    extractProductIdFromResponse(response) {
        // 嘗試從回應中提取商品ID
        if (typeof response === 'string') {
            const match = response.match(/data-product-id="(\d+)"/);
            if (match) {
                return parseInt(match[1]);
            }
        }
        return null;
    }
}

// 自動初始化
$(document).ready(function () {
    window.productManager = new ProductManager();
});

// 輔助函數
function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

function validateImageFile(file, maxSize = 5242880) {
    if (!file.type.startsWith('image/')) {
        return { valid: false, message: '請選擇有效的圖片檔案' };
    }

    if (file.size > maxSize) {
        return {
            valid: false,
            message: `檔案大小不能超過 ${formatFileSize(maxSize)}`
        };
    }

    return { valid: true };
}