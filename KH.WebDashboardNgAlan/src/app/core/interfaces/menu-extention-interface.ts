import { Menu as OriginalMenu } from '@delon/theme';

declare module '@delon/theme' {
    interface MenuX extends OriginalMenu {
        acl?: string | string[];
    }
}