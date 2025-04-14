import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TargetsComponent } from './targets.component';
import { TargetDetailComponent } from './target-detail/target-detail.component';
import { TargetCreateModalContainerComponent } from './target-create/target-create-modal-container.component';
import { TargetEditModalContainerComponent } from './target-edit/target-edit-modal-container.component';
import { TargetDeleteModalContainerComponent } from './target-delete/target-delete-modal-container.component';
import { AuthGuardService } from '../../lib/auth-guard.service';
import { SecureComponent } from '../layouts/secure/secure.component';

const routes: Routes = [
  {
    path: '',
    component: SecureComponent,
    canActivate: [AuthGuardService],
    data: { title: 'Secure Views' },
    children: [
      {
        path: '',
        redirectTo: '/targets',
        pathMatch: 'full',
        data: { preload: true }
      },
      {
        path: 'targets',
        component: TargetsComponent,
        canActivate: [AuthGuardService],
        children: [
          {
            path: '',
            canActivateChild: [AuthGuardService],
            children: [
              {
                path: 'create',
                component: TargetCreateModalContainerComponent
              }
            ]
          }
        ]
      },
      {
        path: 'targets/:id',
        component: TargetDetailComponent,
        canActivate: [AuthGuardService],
        children: [
          {
            path: '',
            canActivateChild: [AuthGuardService],
            children: [
              {
                path: 'edit',
                component: TargetEditModalContainerComponent
              }
            ]
          },
          {
            path: '',
            canActivateChild: [AuthGuardService],
            children: [
              {
                path: 'delete',
                component: TargetDeleteModalContainerComponent
              }
            ]
          }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TargetsRoutingModule { }
