import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonasFisicasComponent } from './personas-fisicas.component';

describe('PersonasFisicasComponent', () => {
  let component: PersonasFisicasComponent;
  let fixture: ComponentFixture<PersonasFisicasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PersonasFisicasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PersonasFisicasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
