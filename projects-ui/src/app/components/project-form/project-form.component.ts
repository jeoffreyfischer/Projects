import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectInfoDTO } from 'src/app/models/project-info-dto.interface';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-project-form',
  templateUrl: './project-form.component.html',
  styleUrls: ['./project-form.component.css']
})
export class ProjectFormComponent implements OnInit {

  isAddMode: boolean = false;

  constructor(private projectService: ProjectService, private route: ActivatedRoute, private router: Router) {
    this.isAddMode = this.route.snapshot.url.toString().includes('add');
  }

  projectForm = new FormGroup({
    id: new FormControl(0),
    name: new FormControl('', [Validators.required, Validators.minLength(2)]),
    author: new FormControl('', [Validators.required, Validators.minLength(2)]),
    description: new FormControl('', [Validators.required, Validators.minLength(2)]),
    sizeInBytes: new FormControl(0, [Validators.required, Validators.min(0)]),
    isCompleted: new FormControl()
  })

  ngOnInit(): void {
    if (!this.isAddMode) {
      const id = this.route.snapshot.params['id'];
      if (id) {
        console.log(id);
        this.projectService.getProject(id).subscribe(
          (project) => {
            this.projectForm.patchValue({
              id: project.id,
              name: project.name,
              author: project.author,
              description: project.description,
              sizeInBytes: project.sizeInBytes,
              isCompleted: project.isCompleted
            })
          }
        )
      }
    }
  }

  goToListPage() {
    this.router.navigate(['/projects']);
  }

  deleteClient() {
    this.projectService.deleteProject(this.route.snapshot.params['id']).subscribe(
      () => {
        this.router.navigate(['/projects']);
      }
    )
  }

  onSubmit() {
    if (this.projectForm.valid) {
      const updatedProject = this.projectForm.value as ProjectInfoDTO;
      updatedProject.name = updatedProject.name.trim();
      updatedProject.author = updatedProject.author.trim();
      updatedProject.description = updatedProject.description.trim();
      updatedProject.sizeInBytes = updatedProject.sizeInBytes;
      updatedProject.isCompleted = updatedProject.isCompleted;
      if (this.isAddMode) {
        this.projectService.addProject(updatedProject)
        .subscribe(
          () => {
            this.router.navigate(['/projects']);
          }
        )
      }
      else {
        const id = this.route.snapshot.params['id'];
        this.projectService.updateProject(id, updatedProject)
        .subscribe(
          () => {
            this.router.navigate(['/projects']);
          }
        )
      }
    }

  }

}
