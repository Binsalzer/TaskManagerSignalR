import React, { useState, useRef, useEffect } from 'react';
import axios from 'axios'
import { HubConnectionBuilder } from "@microsoft/signalr"
import 'bootstrap/dist/css/bootstrap.min.css';
import './Home.css';
import { useAuthentication } from '../AuthenticationContext'

const Home = () => {

    const [text, setText] = useState()

    const [tasks, setTasks] = useState([])

    const connectionRef = useRef()

    const { user } = useAuthentication()

    useEffect(() => {
        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/api/task").build();
            await connection.start();

            connectionRef.current = connection;

            connection.on('taskAdded', task => {
                setTasks(tasks => [...tasks, task]);
            })

            connection.on('taskdeleted', id => {
                setTasks(tasks => tasks.filter(t => t.id !== id))
            })

            connection.on('taskSelected', taskItems => {
                setTasks(tasks=> taskItems)
            })
        }

        connectToHub()

    }, [])


    useEffect(() => {

        const getAll = async () => {
            const { data } = await axios.get('/api/tasks/getalltasks')
            setTasks(data)
        }

        getAll()

    }, [])


    const onTextChange = e => {
        setText(e.target.value)
    }

    const onAddClick = async () => {
        await axios.post('/api/tasks/addtask', { description: text })
        connectionRef.current.invoke('addTask', { description: text })
        setText('')
    }

    const getStatusText = t => {
        if (t.userId === null) {
            return `I'm doing this one`
        } else if (t.userId === user.id) {
            return `i'm done!`
        } else {
            return 'other'
        }
    }

    const getStatusColor = t => {
        if (t.userId === null) {
            return 'btn btn-dark'
        } else if (t.userId === user.id) {
            return 'btn btn-success'
        } else {
            return 'btn btn-warning'
        }
    }

    const onButtonClick = t => {
        if (t.userId === user.id) {
            onDeleteClick(t.id)
        } else {
            onSelectClick(t.id)
        }
    }

    const onDeleteClick = async (id) => {
        await axios.post('/api/tasks/delete', { id })
        connectionRef.current.invoke('delete', id)
    }

    const onSelectClick = async(id) => {
        await axios.post('/api/tasks/select', { id })
        connectionRef.current.invoke('select')
    }

    return (
        <div className="container" style={{ marginTop: '80px' }}>
            <div style={{ marginTop: '70px' }} >
                <div className='row'>
                    <div className="col-md-10">
                        <input type="text" className="form-control" placeholder="Task Title" value={text} onChange={onTextChange}></input>
                    </div>
                    <div className="col-md-2">
                        <button className="btn btn-primary w-100" onClick={onAddClick}>Add Task</button>
                    </div>
                </div>
                <table className="table table-hover table-striped table-bordered mt-3">
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tasks.map(t => <tr key={t.id}>
                            <td>{t.description}</td>
                            <td>
                                <button className={getStatusColor(t)} disabled={t.userId !== user.id && t.userId !== null} onClick={() => { onButtonClick(t) }}>{getStatusText(t)}</button>
                            </td>
                        </tr>)}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default Home;