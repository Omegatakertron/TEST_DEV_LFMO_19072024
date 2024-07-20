import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AgGridModule } from 'ag-grid-angular';
import { IOlympicData } from '../interfaces';
import { ColDef,ColGroupDef,GridApi, GridReadyEvent,SideBarDef,CellEditingStartedEvent,CellEditingStoppedEvent,ICellEditorParams,RowEditingStartedEvent,RowEditingStoppedEvent,CellEditorSelectorResult,} from 'ag-grid-community';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.scss']
})
export class ReportesComponent {

  FData!: GridReadyEvent<IOlympicData>
  private gridApi!: GridApi<IOlympicData>;
  public rowData!: IOlympicData[];
  public rowSelection: 'single' | 'multiple' = 'multiple';
  public defaultColDef: ColDef = {
    flex: 1,
    minWidth: 300,
    resizable: true,
    enableValue: true,
    sortable: true,
    filter: true,
    floatingFilter: true
  };
  public columnDefs: (ColDef | ColGroupDef)[] = [

    { field: "nombreCompleto",pinned: 'left',cellStyle: {fontSize: '12px', fontWeight: 'bold', textAlign:'center', border:'1px solid #bdbdbd'},editable:true},
    { field: 'fechaRegistro',pinned: 'left',cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'}},
    { field: 'fechaAtualizacion',pinned: 'left'},
    { field: 'rfc',pinned: 'left',cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'} },
    { field: 'FechaNacimiento',cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'} },
]

  constructor(private http: HttpClient, private router: Router){

  }

  ngOnInit(){
    if(sessionStorage.getItem('SessionLogged') == null){
      this.router.navigate(["/Reportes"])
    }
  }

  onGridReady(params: GridReadyEvent<IOlympicData>) {
    params.api.setFilterModel(null);
    this.FData = params;
    this.gridApi = params.api;
    let url = "http://localhost:5128/api/ObtenerPersonasFisicas";
    this.http.get<any>(url).subscribe((result) => {
        console.log("Persona Fisica: ", result);
        if (result.success == true) {
          this.rowData = result.data;
        }
      });
  }
}
