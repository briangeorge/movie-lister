import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MovielistDetailComponent } from './movielist-detail.component';

describe('MovielistDetailComponent', () => {
  let component: MovielistDetailComponent;
  let fixture: ComponentFixture<MovielistDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MovielistDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MovielistDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
