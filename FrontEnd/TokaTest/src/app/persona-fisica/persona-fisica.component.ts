import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AgGridModule } from 'ag-grid-angular';
import { IOlympicData } from '../interfaces';
import { ColDef,ColGroupDef,GridApi, GridReadyEvent,SideBarDef,CellEditingStartedEvent,CellEditingStoppedEvent,ICellEditorParams,RowEditingStartedEvent,RowEditingStoppedEvent,CellEditorSelectorResult,} from 'ag-grid-community';

@Component({
  selector: 'app-persona-fisica',
  templateUrl: './persona-fisica.component.html',
  styleUrls: ['./persona-fisica.component.scss']
})
export class PersonaFisicaComponent {

  FData!: GridReadyEvent<IOlympicData>
  private gridApi!: GridApi<IOlympicData>;
  public rowData!: IOlympicData[];
  public rowSelection: 'single' | 'multiple' = 'multiple';
  public defaultColDef: ColDef = {
    flex: 1,
    minWidth: 300,
    resizable: true,
    enableValue: true,
    //enablePivot: true,
    sortable: true,
    //enableRowGroup: true,

    filter: true,
    floatingFilter: true
  };
  public columnDefs: (ColDef | ColGroupDef)[] = [

    { field: "nombreCompleto",pinned: 'left',cellStyle: {fontSize: '12px', fontWeight: 'bold', textAlign:'center', border:'1px solid #bdbdbd'},editable:true},
    { field: 'fechaRegistro',pinned: 'left',cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'}},
    { field: 'fechaAtualizacion',pinned: 'left'},
    { field: 'rfc',pinned: 'left',cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'} },
    { field: 'FechaNacimiento',cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'} },
    { field: 'editar', headerName: 'Editar', cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'}, onCellClicked:params => this.EditPersonaFisica(params.data.idPersonaFisica)},
    { field: 'eliminar', headerName: 'Eliminar', cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'}, onCellClicked:params => this.EliminarPersonaFisica(params.data.idPersonaFisica)},
]
  action: any = 'Añadir';

  constructor(private http:HttpClient, private router: Router){}

  ngOnInit(){
    if(sessionStorage.getItem('SessionLogged') == null){
      this.router.navigate(["/LogIn"])
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
  
  EditPersonaFisica(id:any){
    this.action = 'Editar'


  }

  EliminarPersonaFisica(idPF:any){
    let confirmation = confirm('Esta seguro de esta acción?');
    if(confirmation){
      let url = "http://localhost:5128/api/EliminarPersonaFisica";
      this.http.delete<any>(url).subscribe((result) => {
          console.log("Persona Fisica: ", result);
          if (result.success == true) {
            this.rowData = result.data;
          }
        });
    }
    else{

    }
  }

  ActionPersonaFisica(act:any){
    console.log("Action to do: ", act);
  }
}
