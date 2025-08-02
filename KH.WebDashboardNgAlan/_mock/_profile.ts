const basicGoods = [
  {
    id: '1234561',
    name: 'Mineral Water 550ml',
    barcode: '12421432143214321',
    price: '2.00',
    num: '1',
    amount: '2.00'
  },
  {
    id: '1234562',
    name: 'Herbal Tea 300ml',
    barcode: '12421432143214322',
    price: '3.00',
    num: '2',
    amount: '6.00'
  },
  {
    id: '1234563',
    name: 'Delicious Chips',
    barcode: '12421432143214323',
    price: '7.00',
    num: '4',
    amount: '28.00'
  },
  {
    id: '1234564',
    name: 'Especially Delicious Egg Rolls',
    barcode: '12421432143214324',
    price: '8.50',
    num: '3',
    amount: '25.50'
  }
];

const basicProgress = [
  {
    key: '1',
    time: '2017-10-01 14:10',
    rate: 'Contact Customer',
    status: 'processing',
    operator: 'Pickup Staff ID1234',
    cost: '5mins'
  },
  {
    key: '2',
    time: '2017-10-01 14:05',
    rate: 'Pickup Staff Departure',
    status: 'success',
    operator: 'Pickup Staff ID1234',
    cost: '1h'
  },
  {
    key: '3',
    time: '2017-10-01 13:05',
    rate: 'Pickup Staff Accept Order',
    status: 'success',
    operator: 'Pickup Staff ID1234',
    cost: '5mins'
  },
  {
    key: '4',
    time: '2017-10-01 13:00',
    rate: 'Application Approval Passed',
    status: 'success',
    operator: 'System',
    cost: '1h'
  },
  {
    key: '5',
    time: '2017-10-01 12:00',
    rate: 'Initiate Return Request',
    status: 'success',
    operator: 'User',
    cost: '5mins'
  }
];

const advancedOperation1 = [
  {
    key: 'op1',
    type: 'Purchase Relationship Effective',
    name: 'Owner',
    status: 'agree',
    updatedAt: '2017-10-03  19:23:12',
    memo: '-'
  },
  {
    key: 'op2',
    type: 'Financial Review',
    name: 'Finance Staff',
    status: 'reject',
    updatedAt: '2017-10-03  19:23:12',
    memo: 'Rejection Reason'
  },
  {
    key: 'op3',
    type: 'Department Initial Review',
    name: 'Department Staff',
    status: 'agree',
    updatedAt: '2017-10-03  19:23:12',
    memo: '-'
  },
  {
    key: 'op4',
    type: 'Submit Order',
    name: 'Order Staff',
    status: 'agree',
    updatedAt: '2017-10-03  19:23:12',
    memo: 'Great'
  },
  {
    key: 'op5',
    type: 'Create Order',
    name: 'Order Creator',
    status: 'agree',
    updatedAt: '2017-10-03  19:23:12',
    memo: '-'
  }
];

const advancedOperation2 = [
  {
    key: 'op1',
    type: 'Purchase Relationship Effective',
    name: 'Owner',
    status: 'agree',
    updatedAt: '2017-10-03  19:23:12',
    memo: '-'
  }
];

const advancedOperation3 = [
  {
    key: 'op1',
    type: 'Create Order',
    name: 'Order Creator',
    status: 'agree',
    updatedAt: '2017-10-03  19:23:12',
    memo: '-'
  }
];

export const PROFILES = {
  'GET /profile/progress': basicProgress,
  'GET /profile/goods': basicGoods,
  'GET /profile/advanced': {
    advancedOperation1,
    advancedOperation2,
    advancedOperation3
  }
};
