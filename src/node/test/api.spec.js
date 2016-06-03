var request = require('supertest');
var app = require('../app.js')

// create task


// get list of tasks
describe('GET /tasks', function() {
    it('respond with json', function(done) {
        request(app)
            .get('/api/tasks/')
            .set('Accept', 'application/json')
            .send({some: 'body'})
            .expect('Content-Type', /json/)
            .expect(200, done);
    });
});


//
// describe('POST /user', function() {
//     it('respond with json', function(done) {
//         request(app)
//             .post('/items')
//             .set('Accept', 'application/json')
//             .send({name: 'Yuppers'})
//             .expect('Content-Type', /json/)
//             .expect(200, done);
//     });
// });


/*
create task
get list of tasks
subscribe to task updates
update task
delete task

create -> POST /tasks
get list -> GET /tasks
subscribe -> GET /tasks
update -> PUT /task/1
*/
