var filemanager = {
    obj: null,
    url_GetFiles: '',
    url_CreateFolder: '',
    url_DeleteFolder: '',
    url_RenameFolder: '',
    url_DeleteFile: '',
    url_RenameFile: '',
    url_PreviewFile: '',
    url_UploadFiles: '',
    url_MoveToFolder: '',
    toolbar: {
        createFolder: null,
        deleteFolder: null,
        renameFolder: null,
        deleteFile: null,
        renameFile: null
    },
    viewInNewTab: null,
    expandAll: null,
    collapseAll: null,
    pathContainer: null,
    fileContainer: null,
    previewContainer: null,
    messageContainer: null,
    uploadContainer: null,
    selectedPath: null,
    fileSizeLimit: 0,
    _folderStatus: [],
    bind: function () {
        var me = this;

        me.obj.click(function () {
            me.clearMessage();
        });

        me.toolbar.createFolder.click(function () {
            modalInput('Create New Folder', 'Specify new folder name', function (ret, val) {
                if (ret) {
                    var data = {
                        xPath: me.selectedPath,
                        newFolderName: val
                    };
                    $.post(me.url_CreateFolder, data, function (res) {
                        if (res.IsSuccessful) {
                            me.showMessage(res.Remarks, false);
                            me.show(res.Data);
                        } else {
                            modalMessage(res.Err.split('\n'), 'Create New Folder', true);
                        }
                    })
                }
            });
        });

        me.toolbar.deleteFolder.click(function () {
            modalConfirm('Are you sure you want to delete this folder including all folders and files in it?', function (ret) {
                if (ret) {
                    var data = {
                        xPath: me.selectedPath
                    };
                    $.post(me.url_DeleteFolder, data, function (res) {
                        if (res.IsSuccessful) {
                            me.showMessage(res.Remarks, false);
                            me.show(res.Data);
                        } else {
                            modalMessage(res.Err.split('\n'), 'Delete Folder', true);
                        }
                    })
                }
            });
        });

        me.toolbar.renameFolder.click(function () {
            modalInput('Rename Folder', 'Specify new folder name', function (ret, val) {
                if (ret) {
                    var data = {
                        xPath: me.selectedPath,
                        newFolderName: val
                    };
                    $.post(me.url_RenameFolder, data, function (res) {
                        if (res.IsSuccessful) {
                            me.showMessage(res.Remarks, false);
                            me.show(res.Data);
                        } else {
                            modalMessage(res.Err.split('\n'), 'Rename Folder', true);
                        }
                    })
                }
            });
        });

        me.toolbar.deleteFile.click(function () {
            modalConfirm('Are you sure you want to delete the selected file?', function (ret) {
                if (ret) {
                    var data = {
                        xPath: me.selectedPath
                    };
                    $.post(me.url_DeleteFile, data, function (res) {
                        if (res.IsSuccessful) {
                            me.showMessage(res.Remarks, false);
                            me.show(res.Data);
                        } else {
                            modalMessage(res.Err.split('\n'), 'Delete File', true);
                        }
                    })
                }
            });
        });

        me.toolbar.renameFile.click(function () {
            modalInput('Rename File', 'Specify new filename', function (ret, val) {
                if (ret) {
                    var data = {
                        xPath: me.selectedPath,
                        newFileName: val
                    };
                    $.post(me.url_RenameFile, data, function (res) {
                        if (res.IsSuccessful) {
                            me.showMessage(res.Remarks, false);
                            me.show(res.Data);
                        } else {
                            modalMessage(res.Err.split('\n'), 'Rename File', true);
                        }
                    })
                }
            });
        });

        me.expandAll.click(function (e) {
            e.preventDefault();

            me.fileContainer.find('li.folder').addClass('expanded');
        });

        me.collapseAll.click(function (e) {
            e.preventDefault();

            me.fileContainer.find('li.folder').removeClass('expanded');
        });

    },
    init: function () {
        this.toolbar.createFolder = this.obj.find('.create-folder');
        this.toolbar.deleteFolder = this.obj.find('.delete-folder');
        this.toolbar.renameFolder = this.obj.find('.rename-folder');
        this.toolbar.deleteFile = this.obj.find('.delete-file');
        this.toolbar.renameFile = this.obj.find('.rename-file');

        this.viewInNewTab = this.obj.find('.view-in-newtab');
        this.expandAll = this.obj.find('.expand-all');
        this.collapseAll = this.obj.find('.collapse-all');
        this.pathContainer = this.obj.find('.fm-path');
        this.fileContainer = this.obj.find('.fm-contents');
        this.previewContainer = this.obj.find('.fm-preview');
        this.messageContainer = this.obj.find('.fm-message');
        this.uploadContainer = this.obj.find('.fm-upload');

        this.bind();
        this.setUploadUI();
        this.show(null);
    },
    moveToFolder: function (srcXPath, destXPath) {
        var me = this;
        var data = {
            srcXPath: srcXPath,
            destXPath: destXPath
        };
        $.post(me.url_MoveToFolder, data, function (res) {
            if (res.IsSuccessful) {
                me.showMessage(res.Remarks, false);
                me.show(res.Data);
            } else {
                modalMessage(res.Err.split('\n'), 'Move File', true);
            }
        })
    },
    setDragDropUI: function () {
        var me = this;
        
        me.fileContainer.find('li.file > div, li.folder > div').not('li.fixed-folder > div')
            .attr('draggable', 'true')
            .attr('ondragstart', 'drag(event)');

        me.fileContainer.find('li.folder > div')
            .attr('ondrop', 'drop(event)')
            .attr('ondragover', 'allowDrop(event)');
        
    },
    setUploadUI: function () {
        var me = this;

        var input = me.uploadContainer.find('input');
        input.fileinput({
            theme: 'fa',
            uploadUrl: me.url_UploadFiles,
            uploadExtraData: function () {
                return {
                    xpath: me.selectedPath
                };
            },
            allowedFileExtensions: ["jpg", "pdf"],
            fileActionSettings: {
                showUpload: false
            },
            maxFileSize: me.fileSizeLimit / 1000
        });

        input.on('fileuploaded', function (event, data) {
            
        });

        input.on('filebatchuploadcomplete', function (event, files, extra) {
            me.show(null);
            input.fileinput('clear');
            me.showMessage('Successfully uploaded files');
        });
    },
    setCollapseUI: function () {
        var me = this;
        var trigger = me.fileContainer.find('a.collapse-trigger');

        trigger.click(function (e) {
            e.preventDefault();

            var a = $(this);
            a.closest('li').toggleClass('expanded');
        });
    },
    showMessage: function (msg, isError, dontClear) {
        var me = this;
        var obj;
        if (isError) {
            obj = me.messageContainer.find('.alert-danger');
            if (!dontClear) {
                obj.empty();
            }
            obj.html(msg);
        } else {
            obj = me.messageContainer.find('.alert-success');
            if (!dontClear) {
                obj.empty();
            }

            obj.html(msg);
        }
    },
    clearMessage: function () {
        this.messageContainer.find('.alert').empty();
    },
    setSelectedPath: function (xpath) {
        var me = this;
        var ret = {
            isFile: false,
            isFixedFolder: false
        };
        

        me.fileContainer.find('li').removeClass('selected');
        
        if (xpath) {
            var e = me.fileContainer.find('li[xpath="' + xpath + '"]').first();

            e.addClass('selected');
            me.selectedPath = e.attr('xpath');

            var p = e.attr('path');

            me.pathContainer.html(p);
            ret.isFile = e.hasClass('file');
            ret.isFixedFolder = e.hasClass('fixed-folder');
        } else {
            me.selectedPath = null;
            me.pathContainer.html('\\');
        }

        return ret;
    },
    saveFolderStatus: function () {
        var me = this;

        me._folderStatus = [];
        me.fileContainer.find('li.folder').each(function (i, v) {
            var li = $(v);
            var item = {
                xpath: li.attr('xpath'),
                expanded: li.hasClass('expanded')
            };
            me._folderStatus.push(item);
        });
    },
    getFolderStatus: function () {
        var me = this;
        me._folderStatus.forEach(function (v) {
            var li = me.fileContainer.find('li[xpath="' + v.xpath + '"]');
            if (li.length > 0) {
                li.removeClass('expanded');
                if (v.expanded) li.addClass('expanded');
            }
        });
    },
    show: function (xpath) {
        var me = this;

        me.fileContainer.addClass('loading-mask');

        me.saveFolderStatus();

        me.fileContainer.load(this.url_GetFiles, function () {
            me.setDragDropUI();
            me.setCollapseUI();
            var info = me.setSelectedPath(xpath);

            var setbuttons = function (isFile, isFixedFolder) {
                if (me.selectedPath == null || me.selectedPath == '/ul[1]/li[1]') {
                    me.toolbar.deleteFolder.hide();
                    me.toolbar.renameFolder.hide();
                    me.toolbar.deleteFile.hide();
                    me.toolbar.renameFile.hide();
                } else if (isFile) {
                    me.toolbar.deleteFolder.hide();
                    me.toolbar.renameFolder.hide();
                    me.toolbar.deleteFile.show();
                    me.toolbar.renameFile.show();
                } else {
                    if (isFixedFolder) {
                        me.toolbar.deleteFolder.hide();
                        me.toolbar.renameFolder.hide();
                    } else {
                        me.toolbar.deleteFolder.show();
                        me.toolbar.renameFolder.show();
                    }                    
                    me.toolbar.deleteFile.hide();
                    me.toolbar.renameFile.hide();
                }                
            };

            me.fileContainer.find('li').click(function (e) {
                e.stopPropagation();

                var li = $(this);
                me.setSelectedPath(li.attr('xpath'));

                var isFile = li.hasClass('file');
                var isFixedFolder = li.hasClass('fixed-folder');

                if (isFile) {
                    me.showPreview(li);

                    me.previewContainer.show();
                    me.uploadContainer.hide();
                } else {
                    me.previewContainer.hide();
                    me.uploadContainer.show();
                }

                setbuttons(isFile, isFixedFolder);
            });

            me.getFolderStatus();

            setbuttons(info.isFile, info.isFixedFolder);
            me.fileContainer.removeClass('loading-mask');
        });
    },
    showPreview: function (li) {
        var me = this;
        var isImage = li.hasClass('is-image');
        var isPDF = li.hasClass('is-pdf');

        var url = me.url_PreviewFile + '?xPath=' + me.selectedPath;

        if (isImage) {
            var obj = $('<img />').attr('src', url);            
        } else if (isPDF) {
            var obj = $('<iframe />').attr('src', url);
        }

        me.viewInNewTab.unbind().click(function () {
            window.open(url, '_blank');
        });

        me.previewContainer.find('.contents').empty().append(obj);        
    }
}