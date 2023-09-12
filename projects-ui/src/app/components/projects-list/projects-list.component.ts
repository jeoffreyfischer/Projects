import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, debounceTime, distinctUntilChanged, startWith, switchMap } from 'rxjs';
import { ProjectDisplayDTO } from 'src/app/models/project-display-dto.interface';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.css']
})
export class ProjectsListComponent implements OnInit {

  filteredProjects$: Observable<ProjectDisplayDTO[]> = new Observable<ProjectDisplayDTO[]>();
  searchQueryControl: FormControl;

  constructor(private projectService: ProjectService, private router: Router) {
    this.searchQueryControl = new FormControl('');
  }

  ngOnInit(): void {
    this.filteredProjects$ = this.searchQueryControl.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        startWith(this.searchQueryControl.value),
        switchMap(query => {
          if (!query || query.trim() === '') {
            return this.projectService.getProjects();
          }
          else {
            return this.projectService.searchProjects(query);
          }
        })
      )
  }

  goToAddPage() {
    this.router.navigate(['/projects/add']);
  }

  goToProject(projectId: number) {
    this.router.navigate(['/projects/edit', projectId]);
  }

}
