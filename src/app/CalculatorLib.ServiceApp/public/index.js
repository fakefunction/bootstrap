

var gridAppBuilder = function (opts) {
    opts = opts ||
    {
        gridElement: '',
        page: '',
        controllerData: '',
        controllerSchema: '',
        offlineMode: false,
        hideSelectionBoxColumn: false,
        hideActionsColumn: false,
        gridOption: {
            //headerTemplate:'#headerTemplate',
            //detailTemplate:'#detailTemplate',
            //rowTemplate:'#rowTemplate',
        },
        useInlineEdit: false,
        hideExportControls: false,
        pageSize: 0,
        hidePaging: false,
        preventEditing: false,
        hideGridLines: false,
        hideHeader: false,
        rowSelected: function (arg) {},
        recordDoubleClick: function (arg) { },
        recordClick: function (arg) { }
        };

    var ele = document.getElementById('container');
    if (ele) {
        ele.style.visibility = "visible";
    }
    opts.gridElement = opts. gridElement || '#Grid';
    //var page = "Orders";

    //var hostUrl = 'https://ej2services.syncfusion.com/production/web-services/';
    var hostUrl = '';
    var data = function () {
        var url = (opts.hidePaging && (hostUrl + "apig/" + opts.page + "/" + (opts.controllerData || opts.page) + '/?$inlinecount=allpages&$skip=0&$top=' + opts.pageSize) ) || hostUrl + "apig/" + opts.page + "/" + (opts.controllerData || opts.page) ;
        return new ej.data.DataManager({
            url: url,
            adaptor: (opts.offlineMode && ej.data.RemoteSaveAdaptor())|| new ej.data.WebApiAdaptor(),
            crossDomain: true,
            offline: opts.offlineMode
        });
    };

    $.get("apig/" + opts.page + "/" + (opts.controllerSchema || opts. page)  + "/Get?id=0").done(function (composite) {
        console.log(composite);
        var col = (opts.hideSelectionBoxColumn && []) || [{ type: 'checkbox', allowFiltering: false, allowSorting: false, width: '40' }];

        var fields = composite.Schema.FieldsReadable;
        var replacements = composite.ReplacementSchema;
        if (replacements) {
            for (var j = 0; j < fields.length; j++) {
                 for (var property in replacements) {
                     if (replacements.hasOwnProperty(property)) {
                         var candidate = fields[j]["headerText"] === property;
                         var noSuchExist = fields[j]["headerText"] !== replacements[property];
                         if (candidate && noSuchExist ) {
                             fields[j]["headerText"] = replacements[property];
                    }
                }
              }
            }
        }

            col = col.concat(fields);
        if (!opts.hideActionsColumn) {
            col.push({
            headerText: 'Actions', width: 60,
            commands: [{ type: 'Edit', buttonOption: { iconCss: ' e-icons e-edit', cssClass: 'e-flat' } },
            { type: 'Delete', buttonOption: { iconCss: 'e-icons e-delete', cssClass: 'e-flat' } },
            { type: 'Save', buttonOption: { iconCss: 'e-icons e-update', cssClass: 'e-flat' } },
            { type: 'Cancel', buttonOption: { iconCss: 'e-icons e-cancel-icon', cssClass: 'e-flat' } }]
        });
        }
        //headerTemplate: '#employeetemplate'
        //    rowTemplate: '#rowtemplate',
        //https://ej2.syncfusion.com/demos/#/material/grid/grid-overview.html
        //https://ej2.syncfusion.com/javascript/documentation/grid/columns/?_ga=2.243944193.500114582.1549739902-2018847989.1549546969#column-template
        var editMode = 'Dialog';
        if (opts.useInlineEdit) {
            editMode = 'Inline';
        }

  //function create(args) {

  //          this.getHeaderContent().hide();

  //      }
        var gridAppOpts = {
            dataBound: function (args) {
                //hide Grid header 

                opts.hideHeader && $(this.getHeaderContent()).hide();
               
                //add border to the content at the top 
                // this.getContent().css("border-top", "1px solid #c8c8c8");
            },
            // rowTemplate: "#hello", 
            //showColumnHeaders:false,
            dataSource: data(),
            allowReordering: true,
            allowResizing: true,

            beginEdit: function(args) {
                args.row.classList.add('e-dlgeditrow');
            },
            actionComplete: function(e) {
                if (e.requestType == 'save') {
                    grid.refresh();
                }
                if (e.requestType == 'beginEdit' || e.requestType == 'add') {
                    e.form.querySelector("tr").remove(); // in this sample check box is in first row 
                }
            },
            contextMenuItems: [
                'AutoFit', 'AutoFitAll', 'SortAscending', 'SortDescending',
                'Copy', 'Edit', 'Delete', 'Save', 'Cancel',
                'PdfExport', 'ExcelExport', 'CsvExport', 'FirstPage', 'PrevPage',
                'LastPage', 'NextPage'
            ],
           // create: "create",
            allowExcelExport: true,
            allowPdfExport: true,
            selectionSettings: { type: 'Multiple' },
            allowTextWrap: true,
            allowRowDragAndDrop: true,
            groupSettings: { showGroupedColumn: true },
            showColumnMenu: true,
            //Default,none, Both, Horizontal,Vertical
            gridLines: (!opts.hideGridLines) && 'Default',
            hierarchyPrintMode: 'All',
            allowSorting: true,
            allowFiltering: true,
            filterSettings: { type: 'Excel' /*or Menu */ },
            //allowCsvExport: true,
            editSettings: {
                allowEditing: (!opts.preventEditing) ,
                allowAdding: true,
                allowDeleting: true,
                //mode: 'Normal',
                newRowPosition: 'Top',
                //remove this to edit inline
                mode:editMode,
                showDeleteConfirmDialog: true
            },
            allowPaging: (!opts.hidePaging),
            pageSettings:  {
                pageSize: opts. pageSize|| 10,
                pageSizes: true,
                //enableQueryString: false,
                //currentPage: parseInt((function (name) {
                //    var url = window.location.href;
                //    name = name.replace(/[\[\]]/g, '\\$&');
                //    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                //        results = regex.exec(url);
                //    if (!results) return null;
                //    if (!results[2]) return '';
                //    return decodeURIComponent(results[2].replace(/\+/g, ' '));
                //})('page'), 10) || 1
            },
            toolbar: (opts.hideExportControls && [])|| [
                'Print', 'ExcelExport', 'PdfExport' /*, 'CsvExport' */, 'Add', 'Edit', 'Delete', 'Update', 'Cancel',
                'Search',
                { text: 'Copy', tooltipText: 'Copy', prefixIcon: 'e-copy', id: 'copy' },
                { text: 'Copy With Header', tooltipText: 'Copy With Header', prefixIcon: 'e-copy', id: 'copyHeader' }
            ],

            actionBegin: actionBegin,
            //https://ej2.syncfusion.com/javascript/documentation/grid/columns/?no-cache=1&_ga=2.240296455.500114582.1549739902-2018847989.1549546969#auto-generation
            //https://stackblitz.com/edit/h7qscj-ztdpkr?file=index.js
            columns: col,
            recordDoubleClick: opts.recordDoubleClick || function (args) {
               
                // args.cancel   - Returns the cancel option value.

                // args.model    - Returns the grid model.

                // args.type     - Returns the name of the event.

                //args.currentRowIndex  -Return the index of the row clicked.

                //args.currentRow  -Return the target row.

                //args.currentData  -Return the data of the row.

            },
            rowSelected: opts.rowSelected || function(args) {

            },
            recordClick: opts.recordClick|| function (args) {



                // args.cancel   - Returns the cancel option value.

                // args.model    - Returns the grid model.

                // args.type     - Returns the name of the event.

                //args.currentRowIndex  -Return the index of the row clicked.

                //args.currentRow  -Return the target row.

                //args.currentData  -Return the data of the row.

            },
            toolbarClick: function(args) {
                if (grid.getSelectedRecords().length > 0) {
                    var withHeader = false;
                    if (args.item.id === 'copyHeader') {
                        withHeader = true;
                    }
                    grid.copy(withHeader);
                } else {
                    alert("Please select row before copying");
                }
            }
            //detailTemplate: '#detailtemplate',
            //childGrid: {
            //    dataSource: data('Orders'),
            //    queryString: 'IdForeign',
            //    hierarchyPrintMode: 'All',
            //    columns: col
            //}
        };
      
        $.extend(gridAppOpts, opts.gridOption || {});
        
      
        var grid = new ej.grids.Grid(gridAppOpts);


        grid.appendTo(opts.gridElement);
        grid.toolbarClick = function (args) {
            if (args.item.id === 'Grid_pdfexport') {
                grid.pdfExport();
            }
            if (args.item.id === 'Grid_excelexport') {
                // grid.excelExport();
                grid.excelExport(getExcelExportProperties());
            }
            if (args.item.id === 'Grid_csvexport') {
                grid.csvExport();
            }
        };
        function actionBegin(args) {
            if (args.requestType === 'save') {
                if (grid.pageSettings.currentPage !== 1 && grid.editSettings.newRowPosition === 'Top') {
                    args.index = (grid.pageSettings.currentPage * grid.pageSettings.pageSize) - grid.pageSettings.pageSize;
                } else if (grid.editSettings.newRowPosition === 'Bottom') {
                    args.index = (grid.pageSettings.currentPage * grid.pageSettings.pageSize) - 1;
                }
            }
            
           
                if (args.requestType == "searching") {
                    for (var i = 0; i < this.getColumns().length; i++) {
                        if (this.getColumns()[i].field) {
                            this.searchSettings.fields.push(this.getColumns()[i]
                                .field); //These columns fields will only be included in searching. So  
                        }
                    }
                }


        }

        //var dropDownType = new ej.dropdowns.DropDownList({
        //    dataSource: newRowPosition,
        //    fields: {
        //        text: 'newRowPosition',
        //        value: 'id'
        //    },
        //    value: 'Top',
        //    change: function (e) {
        //        var newRowPosition = e.value;
        //        grid.editSettings.newRowPosition = newRowPosition;
        //    }
        //});

        var date = '';
        date += ((new Date()).getMonth().toString()) + '/' + ((new Date()).getDate().toString());
        date += '/' + ((new Date()).getFullYear().toString());
        function getExcelExportProperties() {
            return {

                //footer: {
                //    footerRows: 8,
                //    rows: [
                //        { cells: [{ colSpan: 6, value: "Thank you for your business!", style: { fontColor: '#C67878', hAlign: 'Center', bold: true } }] },
                //        { cells: [{ colSpan: 6, value: "!Visit Again!", style: { fontColor: '#C67878', hAlign: 'Center', bold: true } }] }
                //    ]
                //},

                fileName: "My excel.xlsx",
                /*
                header: {
                    headerRows: 7,
                    rows: [
                        { index: 1, cells: [{ index: 1, colSpan: 5, value: 'INVOICE', style: { fontColor: '#C25050', fontSize: 25, hAlign: 'Center', bold: true } }] },
                        {
                            index: 3,
                            cells: [
                                { index: 1, colSpan: 2, value: "Advencture Traders", style: { fontColor: '#C67878', fontSize: 15, bold: true } },
                                { index: 4, value: "INVOICE NUMBER", style: { fontColor: '#C67878', bold: true } },
                                { index: 5, value: "DATE", style: { fontColor: '#C67878', bold: true }, width: 150 }
                            ]
                        },
                        {
                            index: 4,
                            cells: [{ index: 1, colSpan: 2, value: "2501 Aerial Center Parkway" },
                            { index: 4, value: 2034 }, { index: 5, value: date, width: 150 }
        
                            ]
                        },
        
                        {
                            index: 5,
                            cells: [
                                { index: 1, colSpan: 2, value: "Tel +1 888.936.8638 Fax +1 919.573.0306" },
                                { index: 4, value: "CUSOTMER ID", style: { fontColor: '#C67878', bold: true } }, { index: 5, value: "TERMS", width: 150, style: { fontColor: '#C67878', bold: true } }
        
                            ]
                        },
                        {
                            index: 6,
                            cells: [
        
                                { index: 4, value: 564 }, { index: 5, value: "Net 30 days", width: 150 }
                            ]
                        }
                    ]
                }
                */
            };
        }
       // dropDownType.appendTo('#newRowPosition');





  //inline on pencil click, dialogue on double click
/*
   var dbClick = true;
    grid.on('dblclick', function() {
         dbClick = true;
    });

    var dialogRenderer = new grid.editModule.renderer.editType.Dialog(grid, grid.serviceLocator);

    grid.editModule.renderer = ej.base.extend(grid.editModule.renderer, {
        update: function (args) {
            if (dbClick) {

                dialogRenderer.update(this.getEditElements(args), args);

                this.parent.setProperties({
                    editSettings: { mode: 'Dialog' }
                }, true);

            } else {
                this.renderer.update(this.getEditElements(args), args);
                this.parent.setProperties({
                    editSettings: { mode: 'Normal' }
                }, true);
            }
             dbClick = false;
            this.convertWidget(args);
        }
    });

 */
    });

  
};