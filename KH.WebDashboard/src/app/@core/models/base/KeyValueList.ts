
export class KeyValueList {
    items: { key: string; value: any; }[];

    constructor() {
        this.items = [];
    }

    addItem(key: string, value: any) {
        this.items.push({ key, value });
    }

    removeItem(key: string) {
        const index = this.items.findIndex(item => item.key === key);
        if (index !== -1) {
            this.items.splice(index, 1);
        }
    }

    getItem(key: string): any {
        const item = this.items.find(item => item.key === key);
        return item ? item.value : undefined;
    }
}
