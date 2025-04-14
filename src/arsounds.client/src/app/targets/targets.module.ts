import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ColorPickerModule } from 'ngx-color-picker';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgWaveformModule } from '../../lib/ng-waveform.module';
import { TargetsRoutingModule } from './targets-routing.module';

import { TargetsComponent } from './targets.component';
import { TargetsListComponent } from './targets-list/targets-list.component';
import { TargetDetailComponent } from './target-detail/target-detail.component';
import { TargetDetailActivateComponent } from './target-detail/target-detail-activate.component';
import { TargetDetailDeactivateComponent } from './target-detail/target-detail-deactivate.component';
import { TargetCreateComponent } from './target-create/target-create.component';
import { TargetCreateModalContainerComponent } from './target-create/target-create-modal-container.component';
import { TargetDeleteComponent } from './target-delete/target-delete.component';
import { TargetDeleteModalContainerComponent } from './target-delete/target-delete-modal-container.component';
import { TargetEditComponent } from './target-edit/target-edit.component';
import { TargetEditModalContainerComponent } from './target-edit/target-edit-modal-container.component';
import { ScrollTrackerDirective } from '../../lib/scroll-tracker.directive';
import { FileInputValueAccessor } from '../../lib/file-input.accessor';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TargetsRoutingModule,
    NgWaveformModule,
    ColorPickerModule,
    NgbModule
  ],
  declarations: [
    TargetsComponent,
    TargetsListComponent,
    TargetDetailComponent,
    TargetDetailActivateComponent,
    TargetDetailDeactivateComponent,
    TargetCreateComponent,
    TargetCreateModalContainerComponent,
    TargetDeleteComponent,
    TargetDeleteModalContainerComponent,
    TargetEditComponent,
    TargetEditModalContainerComponent,
    ScrollTrackerDirective,
    FileInputValueAccessor
  ]
})
export class TargetsModule { }
