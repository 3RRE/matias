function clickButtonNew(button, formId, tableId, uris) {
    var promise = new window.Promise(function (resolve, reject) {
        var $form = $(formId);
        var inputFisrt = $form.find('input[type=text],textarea,select').filter(':visible:first');
        inputFisrt.focus(function () { $(this).select(); });
        var $chkboxChecked = $('tbody input[type="checkbox"]:checked', tableId);
        if ($(button).text() === 'Editar') {
            if ($chkboxChecked.length === 1) {
                var id = $chkboxChecked.val();
                $.ajax({
                    url: uris.GetById.replace(/\{0\}/g, id),
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response !== undefined && response !== null) {
                            $.each(response,
                                function (name, val) {
                                    var $el = $('[name="' + name + '"]');
                                    var type = $el.attr('type');
                                    switch (type) {
                                        case 'checkbox':
                                            $el.attr('checked', 'checked');
                                            break;
                                        case 'radio':
                                            $el.filter('[value="' + val + '"]').attr('checked', 'checked');
                                            break;
                                        case 'datetime':
                                            $el.datepicker("update", window.moment(val).toDate());
                                            break;
                                        case 'file':
                                            val = val !== undefined && val !== null ? val.split("/").pop() : '';
                                            $("#" + $el.attr("name") + "_FileName").val(val);
                                            break;
                                        default:
                                            $el.val(val);
                                    }
                                });


                            $form.closest("div.form-section").removeClass('hidden');
                            $(tableId).closest("div.table-section").addClass('hidden');
                            $('[name="ButtonNew"]').text('Cancelar');
                            resolve(response);
                        }
                    },
                    dataType: "json"
                });
            }

        } else if ($(button).text() === 'Nuevo') {
            $form.closest("div.form-section").removeClass('hidden');
            $(tableId).closest("div.table-section").addClass('hidden');
            $('[name="ButtonNew"]').text('Cancelar');
            resetForm(formId);
            resolve(null);
        } else if ($(button).text() === 'Cancelar') {

            if ($chkboxChecked.length === 1) {
                $('[name="ButtonNew"]').text('Editar');
            } else {
                $('[name="ButtonNew"]').text('Nuevo');
            }
            $form.find('[name="ButtonCancel"]').trigger('click');
            resolve(null);
        }


    });
    return promise;
}

function clickButtonCancel(formId, tableId) {
    var promise = new window.Promise(function (resolve) {
        var $form = $(formId);

        var $chkboxChecked = $('tbody input[type="checkbox"]:checked', tableId);
        if ($chkboxChecked.length === 1) {
            $('[name="ButtonNew"]').text('Editar');
        } else {
            $('[name="ButtonNew"]').text('Nuevo');
        }
        $form.closest("div.form-section").addClass('hidden');
        $(tableId).closest("div.table-section").removeClass('hidden');
        resetForm(formId);
        resolve(null);
    });
    return promise;
}

//
// Updates "Select all" control in a data table
//
function updateDataTableSelectAllCtrl(table) {
    var $table = table.table().node();
    var $chkboxAll = $('tbody input[type="checkbox"]', $table);
    var $chkboxChecked = $('tbody input[type="checkbox"]:checked', $table);
    var chkboxSelectAll = $('thead input[name="select_all"]', $table).get(0);

    // If none of the checkboxes are checked
    if ($chkboxChecked.length === 0) {
        chkboxSelectAll.checked = false;
        if ('indeterminate' in chkboxSelectAll) {
            chkboxSelectAll.indeterminate = false;
        }

        // If all of the checkboxes are checked
    } else if ($chkboxChecked.length === $chkboxAll.length) {
        chkboxSelectAll.checked = true;
        if ('indeterminate' in chkboxSelectAll) {
            chkboxSelectAll.indeterminate = false;
        }

        // If some of the checkboxes are checked
    } else {
        chkboxSelectAll.checked = true;
        if ('indeterminate' in chkboxSelectAll) {
            chkboxSelectAll.indeterminate = true;
        }
    }
}

//
// Updates "Select all" control in a data table
//
function updateRightSideBarButtons(table) {
    var $table = table.table().node();
    // var $chkboxAll = $('tbody input[type="checkbox"]', $table);
    var $chkboxChecked = $('tbody input[type="checkbox"]:checked', $table);
    // var chkboxSelectAll = $('thead input[name="select_all"]', $table).get(0);
    // If none of the checkboxes are checked
    if ($chkboxChecked.length === 0) {
        $('[name="ButtonNew"]').text('Nuevo');
        $('[name="ButtonActive"]').addClass('hide');
        $('[name="ButtonDeactivate"]').addClass('hide');
        if ($('[name="ButtonDelete"]').length) {
            $('[name="ButtonDelete"]').addClass('hide');
        }
    } else if ($chkboxChecked.length === 1) {
        $('[name="ButtonNew"]').text('Editar');
        $('[name="ButtonActive"]').removeClass('hide');
        $('[name="ButtonDeactivate"]').removeClass('hide');
        if ($('[name="ButtonDelete"]').length) {
            $('[name="ButtonDelete"]').removeClass('hide');
        }
    } else if ($chkboxChecked.length > 1) {
        $('[name="ButtonNew"]').text('Nuevo');
        $('[name="ButtonActive"]').removeClass('hide');
        $('[name="ButtonDeactivate"]').removeClass('hide');
        if ($('[name="ButtonDelete"]').length) {
            $('[name="ButtonDelete"]').removeClass('hide');
        }
    }
}

function ActivateItems(table, url, status) {
    var promise = new window.Promise(function (resolve, reject) {
        var ids = [];
        var $table = table.table().node();
        var ckeckboxSelected = $('tbody input[type="checkbox"]:checked', $table);
        ckeckboxSelected.each(function (index, elem) {
            ids.push($(elem).val());
        });

        if (ids.length > 0) {
            $.ajax({
                method: 'PUT',
                url: url.replace(/\/$/, "") + '/' + status,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(ids),
                success: function (response) {
                    console.log(response);
                    if (response !== undefined && response !== null) {

                        ckeckboxSelected.each(function (index, elem) {

                            var $td = $(elem)
                                .closest("tr")
                                .find('td.activo');

                            table.cell($td).data(!table.cell($td).data());

                            $td
                                .html('<span class="label ' +
                                    (status ? 'label-success' : 'label-danger') +
                                    '">' +
                                    (status ? 'Activo' : 'No activo') +
                                    '</span>');
                        });

                        if (response.message !== undefined) {
                            window.toastr["success"](response.message);
                        } else {
                            window.toastr["success"]("Los cambios se resolvieron con exito");
                        }
                        resolve(response);
                    }
                },

                dataType: "json"
            });
        }
    });
    return promise;
}

function DeleteItems(table, url) {
    $.LoadingOverlay("show");
    var promise = new window.Promise(function (resolve, reject) {
        var ids = [];
        var $table = table.table().node();
        var ckeckboxSelected = $('tbody input[type="checkbox"]:checked', $table);
        ckeckboxSelected.each(function (index, elem) {
            ids.push($(elem).val());
        });

        if (ids.length > 0) {
            $.ajax({
                method: 'PUT',
                url: url.replace(/\/$/, ""),
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(ids),
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response !== undefined && response !== null) {

                        if (response.message !== undefined) {
                            window.toastr["success"](response.message);
                        } else {
                            window.toastr["success"]("Los cambios se resolvieron con exito");
                        }
                        table.ajax.reload(null, false);
                        $.LoadingOverlay("hide");
                        resolve(response);
                    }
                },
            });
        }
    });
    return promise;
}

function ExportToExcel1(data, uri) {
    var promise = new window.Promise(function (resolve, reject) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', uri, true);
        xhr.responseType = 'blob';
        xhr.setRequestHeader('Content-type', 'application/json;charset=UTF-8');
        xhr.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('token'));
        xhr.onload = function () {
            // Muestra todos los headers en la consola
            console.log("Response Headers:", xhr.getAllResponseHeaders());

            if (this.status === 200) {
                var filename = null;
                var disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var matches = filenameRegex.exec(disposition);
                    if (matches !== null && matches[1]) {
                        filename = matches[1].replace(/['"]/g, '');
                    }
                }
                var type = xhr.getResponseHeader('Content-Type');
                var blob = new Blob([this.response], { type: type });
                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = filename;
                document.body.appendChild(a);
                a.click();
                resolve(this.response);
            }
        };
        xhr.onerror = function () {
            reject(this.response);
        };
        xhr.send(JSON.stringify(data));
        // xhr.send($.param(data));
    });
    return promise;
}

function ExportToWord(data, uri) {
    $.LoadingOverlay("show");
    var promise = new window.Promise(function (resolve, reject) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', uri, true);
        xhr.responseType = 'blob';
        xhr.setRequestHeader('Content-type', 'application/json;charset=UTF-8');
        xhr.setRequestHeader('Authorization', 'Basic ' + localStorage.getItem('token'));
        xhr.onload = function () {
            if (this.status === 200) {
                var type = xhr.getResponseHeader('Content-Type');
                var blob = new Blob([this.response], { type: type });
                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = data.Filename;
                document.body.appendChild(a);
                $.LoadingOverlay("hide");
                a.click();
                resolve(this.response);
            }
        };
        xhr.onerror = function () {
            reject(this.response);
        };
        xhr.send(JSON.stringify(data));
    });

    return promise;
}


function handleClickOnCheckbox(tableId, primaryKey, rowsSelected) {
    // Handle click on checkbox
    $(tableId + ' tbody')
        .on('click',
            'input[type="checkbox"]',
            function (e) {
                var $row = $(this).closest('tr');

                // Get row data
                var data = $(tableId).DataTable().row($row).data();

                // Get row ID
                //var rowId = data.EntityKey;
                var rowId = data[primaryKey];

                // Determine whether row ID is in the list of selected row IDs 
                var index = $.inArray(rowId, rowsSelected);

                // If checkbox is checked and row ID is not in list of selected row IDs
                if (this.checked && index === -1) {
                    rowsSelected.push(rowId);

                    // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
                } else if (!this.checked && index !== -1) {
                    rowsSelected.splice(index, 1);
                }

                if (this.checked) {
                    $row.addClass('selected');
                } else {
                    $row.removeClass('selected');
                }

                // Update state of "Select all" control
                updateDataTableSelectAllCtrl($(tableId).DataTable());
                updateRightSideBarButtons($(tableId).DataTable());
                // Prevent click event from propagating to parent
                e.stopPropagation();
            });

    // Handle click on table cells with checkboxes
    $(tableId)
        .on('click',
            'tbody td, thead th:first-child',
            function () {
                $(this).parent().find('input[type="checkbox"]').trigger('click');
            });

    // Handle click on "Select all" control
    $('thead input[name="select_all"]', $(tableId).DataTable().table().container()).on('click', function (e) {
        //$(tableId).on('click', ' thead input[name="select_all"]', function (e) {
        if (this.checked) {
            $(tableId + ' tbody input[type="checkbox"]:not(:checked)').trigger('click');
        } else {
            $(tableId + ' tbody input[type="checkbox"]:checked').trigger('click');
        }
        // Prevent click event from propagating to parent
        e.stopPropagation();
    });

    // Handle table draw event
    $(tableId).DataTable().on('draw', function () {
        // Update state of "Select all" control
        updateDataTableSelectAllCtrl($(tableId).DataTable());
        //updateRightSideBarButtons($(tableId).DataTable());
    });
}

/* Create an array with the values of all the input boxes in a column */
$.fn.dataTable.ext.order['dom-text'] = function (settings, col) {
    return this.api()
        .column(col, { order: 'index' })
        .nodes()
        .map(function (td) {
            return $('input', td).val();
        });
};

/* Create an array with the values of all the input boxes in a column, parsed as numbers */
$.fn.dataTable.ext.order['dom-text-numeric'] = function (settings, col) {
    return this.api()
        .column(col, { order: 'index' })
        .nodes()
        .map(function (td) {
            return $('input', td).val() * 1;
        });
};

/* Create an array with the values of all the select options in a column */
$.fn.dataTable.ext.order['dom-select'] = function (settings, col) {
    return this.api()
        .column(col, { order: 'index' })
        .nodes()
        .map(function (td) {
            return $('select', td).val();
        });
};

/* Create an array with the values of all the checkboxes in a column */
$.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
    return this.api()
        .column(col, { order: 'index' })
        .nodes()
        .map(function (td) {
            return $('input', td).prop('checked') ? '1' : '0';
        });
};


$.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
    console.log(message);
};