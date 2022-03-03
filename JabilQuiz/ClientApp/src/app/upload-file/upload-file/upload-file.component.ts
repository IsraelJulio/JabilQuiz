
import { Component, OnInit } from '@angular/core';
import { UploadFileService } from '../upload-file.service';
import { take } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { HttpEventType, HttpEvent } from '@angular/common/http';
import { filterResponse, uploadProgress } from '../../shared/rxjs-operators';
import { Quiz } from 'src/app/models/quiz';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css']
})

export class UploadFileComponent implements OnInit {

  files: Set<File>;
  progress = 0;
  quizUrl = 'http://localhost:3000/quiz';
  quizList: Quiz[];

  constructor(private service: UploadFileService) { }

  ngOnInit() { }

  onChange(event) {
    console.log(event);

    const selectedFiles = <FileList>event.srcElement.files;
    // document.getElementById('customFileLabel').innerHTML = selectedFiles[0].name;

    const fileNames = [];
    this.files = new Set();
    for (let i = 0; i < selectedFiles.length; i++) {
      fileNames.push(selectedFiles[i].name);
      this.files.add(selectedFiles[i]);
    }
    document.getElementById('customFileLabel').innerHTML = fileNames.join(', ');

    this.progress = 0;
  }

  onUpload() {
    if (this.files && this.files.size > 0) {
      this.service.upload(this.files,  'api/Upload/UploadFiles')
        .pipe(
          uploadProgress(progress => {
            console.log(progress);
            this.progress = progress;
          }),
          filterResponse()
        )
        .subscribe(response => {
          // this.quizList = response as Quiz[];
          this.service.Save(response,this.quizUrl)
        });
        // .subscribe((event: HttpEvent<Object>) => {
        //   // console.log(event);
        //   if (event.type === HttpEventType.Response) {
        //     console.log('Upload Concluído');
        //   } else if (event.type === HttpEventType.UploadProgress) {
        //     const percentDone = Math.round((event.loaded * 100) / event.total);
        //     // console.log('Progresso', percentDone);
        //     this.progress = percentDone;
        //   }
        // } );
    }
  }

  onDownloadExcel() {
    this.service.download('/api' + '/downloadExcel')
    .subscribe((res: any) => {
      this.service.handleFile(res, 'report.xlsx');
    });
  }

  onDownloadPDF() {
    this.service.download('/api' + '/downloadPDF')
    .subscribe((res: any) => {
      this.service.handleFile(res, 'report.pdf');
    });
  }
}
