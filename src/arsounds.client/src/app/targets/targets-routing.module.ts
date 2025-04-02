import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TargetsComponent } from './targets/targets.component';
import { TargetDetailComponent } from './target-detail/target-detail.component';
import { TargetDeleteModalContainerComponent } from './target-delete/target-delete-modal-container.component';
import { TargetEditModalContainerComponent } from './target-edit/target-edit-modal-container.component';

import { AuthGuardService } from '../../services/auth/auth-guard.service';
import { SecureComponent } from '../layouts/secure/secure.component';

const routes: Routes = [
  {
    path: '', component: SecureComponent, canActivate: [AuthGuardService], data: { title: 'Secure Views' }, children: [
      { path: '', redirectTo: '/targets', pathMatch: 'full', data: { preload: true } },
      {
        path: 'targets',
        component: TargetsComponent,
        canActivate: [AuthGuardService]
      },
      {
        path: 'target/:id',
        component: TargetDetailComponent,
        canActivate: [AuthGuardService],
        children: [
          {
            path: '',
            canActivateChild: [AuthGuardService],
            children: [
              { path: 'delete', component: TargetDeleteModalContainerComponent },
            ]
          },
          {
            path: '',
            canActivateChild: [AuthGuardService],
            children: [
              { path: 'edit', component: TargetEditModalContainerComponent },
            ]
          }
        ]
      },
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class TargetsRoutingModule { }
