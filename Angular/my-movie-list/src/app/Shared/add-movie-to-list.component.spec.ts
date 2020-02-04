import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMovieToListComponent } from './add-movie-to-list.component';

describe('AddMovieToListComponent', () => {
  let component: AddMovieToListComponent;
  let fixture: ComponentFixture<AddMovieToListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMovieToListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMovieToListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
