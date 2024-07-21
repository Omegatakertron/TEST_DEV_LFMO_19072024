import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgModel } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from '../app.component';
import { AgGridModule } from 'ag-grid-angular';
import { IOlympicData } from '../interfaces';
import { ColDef,ColGroupDef,GridApi, GridReadyEvent,SideBarDef,CellEditingStartedEvent,CellEditingStoppedEvent,ICellEditorParams,RowEditingStartedEvent,RowEditingStoppedEvent,CellEditorSelectorResult,} from 'ag-grid-community';
import { window } from 'rxjs';

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
    { field: 'editar', headerName: 'Editar', cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'}, onCellClicked:params => this.Edit(params.data.idPersonaFisica)},
    { field: 'eliminar', headerName: 'Eliminar', cellStyle: {fontSize: '12px', textAlign:'center', border:'1px solid #bdbdbd'}, onCellClicked:params => this.EliminarPersonaFisica(params.data.idPersonaFisica)},
  ]

  action: any = 'Añadir';
  nombre:string = '';
  apellidoPaterno:string = '';
  apellidoMaterno:string = '';
  fechaNacimiento: string = '';
  rfc: string = '';

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
    let url = "http://localhost:5128/api/PersonaFisica/ObtenerPersonasFisicas";
    this.http.get<any>(url).subscribe((result) => {
        console.log("Persona Fisica: ", result);
        if (result.success == true) {
          this.rowData = result.data;
        }
      });
  }
  
  Edit(id:any){
    this.action = 'Editar'
    document.getElementById('PfButton')?.click();
    let boton: any = document.getElementById('registroButton');
    boton.nativeElement.addEventListener('click', this.EditPersonaFisica(id));
  }
  EditPersonaFisica(id:any){
    if(this.nombre != '' && this.apellidoPaterno != '' && this.apellidoPaterno != '' && this.fechaNacimiento != '' && this.rfc != ''){
      let confirmation = confirm('Desea actualizar el registro?');
      if(confirmation){
        let url = "http://localhost:5128/api/PersonaFisica/ActualizarPersonaFisica/{id}";
        const headers = new HttpHeaders({'Content-Type': 'application/json' })
        this.http.put(url,{Nombre: this.nombre, ApellidoPaterno: this.apellidoPaterno, ApellidoMaterno: this.apellidoMaterno, rfc: this.rfc, FechaNacimiento: this.fechaNacimiento, UsuarioAgrega: sessionStorage.getItem('IdAutenticador')}, {headers}).subscribe((result) => {
            console.log("Persona Fisica: ", result);
            if (result) {
              this.onGridReady(this.FData);
            }
          });
      }
    }else{
      alert('Hay algunos campos vacios o faltantes de ifnormción');
    }

  }

  EliminarPersonaFisica(idPF:any){
    let confirmation = confirm('Esta seguro de esta acción?');
    if(confirmation){
    
    }
    else{

    }
  }

  ActionPersonaFisica(act:any){
    if(act == 'Añadir'){
      if(this.nombre != '' && this.apellidoPaterno != '' && this.apellidoPaterno != '' && this.fechaNacimiento != '' && this.rfc != ''){
        let confirmation = confirm('Desea añadir el registro?');
        if(confirmation){
        let url = "http://localhost:5128/api/PersonaFisica/RegistrarPersonaFisica";
        this.http.post<any>(url, {Nombre: this.nombre, ApellidoPaterno: this.apellidoPaterno, ApellidoMaterno: this.apellidoMaterno, rfc: this.rfc, FechaNacimiento: this.fechaNacimiento, UsuarioAgrega: sessionStorage.getItem('IdAutenticador')}).subscribe((result) => {
            console.log("Persona Fisica: ", result);
            if (result.success == true) {
              alert('Registro agregado');
              this.onGridReady(this.FData);
              location.reload();
            }else{
              alert("Algo ha ido mal, intente de nuevo");
            }
          });
        }
      }else{
        alert('Hay algunos campos vacios o faltantes de ifnormción');
      }
    }
  }
}
