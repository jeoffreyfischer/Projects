import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectInfoDTO } from '../models/project-info-dto.interface';
import { ProjectDisplayDTO } from '../models/project-display-dto.interface';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private projectsURL = "http://localhost:5269/Project";

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getProjects(): Observable<ProjectDisplayDTO[]> {
    return this.http.get<ProjectDisplayDTO[]>(`${this.projectsURL}/GetAll`, this.httpOptions);
  }

  getProject(id: number): Observable<ProjectInfoDTO> {
    return this.http.get<ProjectInfoDTO>(`${this.projectsURL}/Get/${id}`, this.httpOptions);
  }

  addProject(project: ProjectInfoDTO): Observable<HttpResponse<any>> {
    return this.http.post<HttpResponse<any>>(`${this.projectsURL}/Add`, project, this.httpOptions);
  }

  updateProject(id: number, project: ProjectInfoDTO): Observable<HttpResponse<any>> {
    return this.http.put<HttpResponse<any>>(`${this.projectsURL}/Edit/${id}`, project, this.httpOptions);
  }

  deleteProject(id: number): Observable<HttpResponse<any>> {
    return this.http.delete<HttpResponse<any>>(`${this.projectsURL}/Delete/${id}`, this.httpOptions);
  }

  searchProjects(query: string): Observable<ProjectInfoDTO[]> {
    return this.http.get<ProjectInfoDTO[]>(`${this.projectsURL}/Search?searchTerm=${query}`, this.httpOptions);
  }

}
